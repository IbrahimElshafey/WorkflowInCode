﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using ResumableFunction.Abstraction;
using ResumableFunction.Abstraction.InOuts;
using ResumableFunction.Engine.Abstraction;
using ResumableFunction.Engine.InOuts;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ResumableFunction.Engine
{
    public partial class FunctionEngine
    {

        public async Task WhenProviderPushEvent(PushedEvent pushedEvent)
        {
            //engine search waits list with(ProviderName, EventType)
            var matchedWaits = await _waitsRepository.GetMatchedWaits(pushedEvent);
            foreach (var wait in matchedWaits)
            {
                var functionClass = new ResumableFunctionWrapper(wait);
                functionClass.UpdateDataWithEventData();
                //get next event wait
                var waitResult = await functionClass.GetNextWait();
                await WaitRequested(waitResult, functionClass);
                await _functionRepository.SaveFunctionState(wait.FunctionRuntimeInfo);
                //* call EventProvider.UnSubscribeEvent(pushedEvent.EventData) if no other intances waits this type for the same provider
            }
        }

        /// <summary>
        /// Will execueted when a function instance run and ask for EventWaiting.
        /// </summary>
        private async Task WaitRequested(NextWaitResult waitResult, ResumableFunctionWrapper functionClass)
        {
            if (waitResult.Result is null && waitResult.IsFinalExit)
            {
                await _functionRepository.MoveFunctionToRecycleBin(functionClass.FunctionRuntimeInfo);
                return;
            }
            if (waitResult.Result != null)
                await WaitRequested(waitResult.Result, functionClass);
        }

        private async Task WaitRequested(Wait waitResult, ResumableFunctionWrapper functionClass)
        {
            switch (waitResult)
            {
                case EventWait eventWait:
                    await EventWaitRequested(eventWait, functionClass);
                    break;
                case ManyWaits manyWaits:
                    await ManyWaitsRequested(manyWaits, functionClass);
                    break;
                case FunctionWait functionWait:
                    await FunctionWaitRequested(functionWait, functionClass);
                    break;
                case ManyFunctionsWait manyFunctionsWait:
                    await ManyFunctionsWaitRequested(manyFunctionsWait, functionClass);
                    break;
            }
        }

        private async Task ManyFunctionsWaitRequested(ManyFunctionsWait functionsWait, ResumableFunctionWrapper functionClass)
        {
            foreach (var function in functionsWait.WaitingFunctions)
            {
                await FunctionWaitRequested(function, functionClass);
            }
            await _waitsRepository.AddWait(functionsWait);
        }

        private async Task FunctionWaitRequested(FunctionWait functionWait, ResumableFunctionWrapper functionClass)
        {
            await WaitRequested(functionWait.CurrentEvent, functionClass);
            await _waitsRepository.AddWait(functionWait);
        }

        private async Task ManyWaitsRequested(ManyWaits manyWaits, ResumableFunctionWrapper functionClass)
        {
            foreach (var eventWait in manyWaits.WaitingEvents)
            {
                await EventWaitRequested(eventWait, functionClass);
            }
            await _waitsRepository.AddWait(manyWaits);
        }

        private async Task EventWaitRequested(EventWait eventWait, ResumableFunctionWrapper functionClass)
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
