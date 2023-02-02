﻿using ResumableFunction.Abstraction.InOuts;
using ResumableFunction.Engine.InOuts;

namespace ResumableFunction.Engine
{
    public partial class FunctionEngine
    {

        public async Task<bool> WhenProviderPushEvent(PushedEvent pushedEvent)
        {
            //engine search waits list with(ProviderName, EventType)
            //matched waits contains Data from pushed event
            //and FunctionRuntimeInfo loaded from database
            var matchedWaits = await _waitsRepository.GetMatchedWaits(pushedEvent);
            foreach (var currentWait in matchedWaits)
            {
                await HandlePushedEvent(currentWait);
                await _functionRepository.SaveFunctionState(currentWait.FunctionRuntimeInfo);

            }
            return false;
        }

        private async Task HandlePushedEvent(EventWait currentWait)
        {
            var functionClass = new ResumableFunctionWrapper(currentWait);
            currentWait.UpdateFunctionData();
            functionClass.FunctionClassInstance = currentWait.CurrntFunction;

            if (IsSingleEvent(currentWait) || await IsGroupLastWait(currentWait))
            {
                //get next event wait
                var nextWaitResult = await functionClass.GetNextWait();
                await HandleNextWait(nextWaitResult, currentWait, functionClass);
                //todo:* call EventProvider.UnSubscribeEvent(pushedEvent.EventData) if no other intances waits this type for the same provider
            }
        }

        private async Task<bool> HandleNextWait(NextWaitResult nextWaitResult, Wait currentWait, ResumableFunctionWrapper functionClass)
        {
            // return await HandleNextWait(nextWaitAftreBacktoCaller, lastFunctionWait, functionClass);
            if (IsFinalExit(nextWaitResult))
                return await _functionRepository.MoveFunctionToRecycleBin(currentWait.FunctionRuntimeInfo);
            else if (IsSubFunctionExit(nextWaitResult))
            {
                return await SubFunctionExit(currentWait, functionClass);
            }
            else if (nextWaitResult.Result is not null)
            {
                //this may cause and error in case of 
                nextWaitResult.Result.ParentFunctionWaitId = currentWait.ParentFunctionWaitId;
                if (nextWaitResult.Result.ReplayType != null)
                    return await ReplayWait(nextWaitResult.Result);
                else
                    return await GenericWaitRequested(nextWaitResult.Result);
            }
            currentWait.Status = WaitStatus.Completed;
            return false;
        }



        private async Task<bool> SubFunctionExit(Wait lastFunctionWait, ResumableFunctionWrapper functionClass)
        {
            //lastFunctionWait =  last function wait before exsit
            var parentFunctionWait = await _waitsRepository.GetParentFunctionWait(lastFunctionWait.ParentFunctionWaitId);
            var backToCaller = false;
            switch (parentFunctionWait)
            {
                //one sub function -> return to caller after function end
                case FunctionWait:
                    backToCaller = true;
                    break;
                //many sub functions -> wait function group to complete and return to caller
                case AllFunctionsWait allFunctionsWait:
                    allFunctionsWait.MoveToMatched(lastFunctionWait.ParentFunctionWaitId);
                    if (allFunctionsWait.Status == WaitStatus.Completed)
                        backToCaller = true;
                    break;
                case AnyFunctionWait anyFunctionWait:
                    anyFunctionWait.SetMatchedFunction(lastFunctionWait.ParentFunctionWaitId);
                    if (anyFunctionWait.Status == WaitStatus.Completed)
                        backToCaller = true;
                    break;
            }

            if (backToCaller)
            {
                var nextWaitAftreBacktoCaller = await functionClass.BackToCaller(parentFunctionWait);
                return await HandleNextWait(nextWaitAftreBacktoCaller, lastFunctionWait, functionClass);
            }

            return true;
        }

        private bool IsFinalExit(NextWaitResult nextWait)
        {
            return nextWait.Result is null && nextWait.IsFinalExit;
        }
        private bool IsSubFunctionExit(NextWaitResult nextWait)
        {
            return nextWait.Result is null && nextWait.IsSubFunctionExit;
        }

        private async Task<bool> IsGroupLastWait(EventWait currentWait)
        {
            var group = await _waitsRepository.GetWaitGroup(currentWait.ParentGroupId);
            switch (group)
            {
                case AllEventsWait allEventsWait:
                    allEventsWait.MoveToMatched(currentWait);
                    return allEventsWait.Status == WaitStatus.Completed;

                case AnyEventWait anyEventWait:
                    anyEventWait.SetMatchedEvent(currentWait); ;
                    return true;
            }
            return false;
        }

        private bool IsSingleEvent(EventWait currentWait)
        {
            return currentWait.ParentGroupId is null;
        }

        private bool Validate(Wait nextWait)
        {
            return true;
        }

        /// <summary>
        /// Will execueted when a function instance run and ask for EventWaiting.
        /// </summary>
        private async Task<bool> GenericWaitRequested(Wait newWait)
        {
            newWait.Status = WaitStatus.Waiting;
            if (Validate(newWait) is false) return false;
            switch (newWait)
            {
                case EventWait eventWait:
                    await SingleWaitRequested(eventWait);
                    break;
                case ManyWaits manyWaits:
                    await ManyWaitsRequested(manyWaits);
                    break;
                case FunctionWait functionWait:
                    await FunctionWaitRequested(functionWait);
                    break;
                case ManyFunctionsWait manyFunctionsWait:
                    await ManyFunctionsWaitRequested(manyFunctionsWait);
                    break;
            }
            return false;
        }

        private async Task<bool> ReplayWait(Wait oldCompletedWait)
        {
            switch (oldCompletedWait.ReplayType)
            {
                case ReplayType.ExecuteDontWait:
                    await Replay(oldCompletedWait);
                    break;
                case ReplayType.WaitSameEventAgain:
                    //oldCompletedWait.ReplayType = null;
                    await GenericWaitRequested(oldCompletedWait);
                    break;
                default:
                    throw new Exception("ReplayWait exception.");
            }
            return true;
        }

        private async Task Replay(Wait oldCompletedWait)
        {
            var functionClass = new ResumableFunctionWrapper(oldCompletedWait)
            {
                FunctionClassInstance = oldCompletedWait.CurrntFunction
            };
            var nextWaitResult = await functionClass.GetNextWait();
            await HandleNextWait(nextWaitResult, oldCompletedWait, functionClass);
        }

        private async Task ManyFunctionsWaitRequested(ManyFunctionsWait functionsWait)
        {
            foreach (var functionWait in functionsWait.WaitingFunctions)
            {
                functionsWait.Status = WaitStatus.Waiting;
                await FunctionWaitRequested(functionWait);
            }
            await _waitsRepository.AddWait(functionsWait);
        }

        private async Task FunctionWaitRequested(FunctionWait functionWait)
        {
            if (functionWait.CurrentWait.ReplayType != null)
                await ReplayWait(functionWait.CurrentWait);
            else
                await GenericWaitRequested(functionWait.CurrentWait);
            await _waitsRepository.AddWait(functionWait);
        }

        private async Task ManyWaitsRequested(ManyWaits manyWaits)
        {
            foreach (var eventWait in manyWaits.WaitingEvents)
            {
                eventWait.Status = WaitStatus.Waiting;
                await SingleWaitRequested(eventWait);
            }
            await _waitsRepository.AddWait(manyWaits);
        }

        private async Task SingleWaitRequested(EventWait eventWait)
        {
            // * Rerwite match expression and set prop expresssion
            eventWait.MatchExpression = new RewriteMatchExpression(eventWait).Result;
            eventWait.SetDataExpression = new RewriteSetDataExpression(eventWait).Result;
            // * Find event provider handler or load it.
            var eventProviderHandler = await _eventProviderRepository.GetByName(eventWait.EventProviderName);
            // * Start event provider if not started 
            await eventProviderHandler.Start();
            // * Call SubscribeToEvent with current paylaod type (eventWaiting.EventData)
            await eventProviderHandler.SubscribeToEvent(eventWait.EventData);
            // * Save event to IActiveEventsRepository 
            await _waitsRepository.AddWait(eventWait);
            // ** important ?? must we send some of SingleEventWaiting props to event provider?? this will make filtering more accurate
            // but the provider will send this data back
        }


    }
}
