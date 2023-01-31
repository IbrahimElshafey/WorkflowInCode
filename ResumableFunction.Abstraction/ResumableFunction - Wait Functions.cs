﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using ResumableFunction.Abstraction.InOuts;

namespace ResumableFunction.Abstraction
{

    public abstract partial class ResumableFunction<FunctionData>
    {
        protected async Task<FunctionWait> Function(string eventIdentifier, Expression<Func<IAsyncEnumerable<Wait>>> function, [CallerMemberName] string callerName = "")
        {
            var result = new FunctionWait(eventIdentifier, function)
            {
                InitiatedByFunction = callerName
            };
            var methodCall = result.Function.Body as MethodCallExpression;
            if (methodCall != null)
            {
                result.FunctionName = methodCall.Method.Name;
            }
            var funcCompiled = result.Function.Compile()();
            if (funcCompiled != null)
            {
                var asyncEnumerator = funcCompiled.GetAsyncEnumerator();
                await asyncEnumerator.MoveNextAsync();
                var firstWait = asyncEnumerator.Current;
                result.CurrentEvent = firstWait;
            }
            return result;
        }
        protected async Task<AllFunctionsWait> Functions
            (string eventIdentifier, Expression<Func<IAsyncEnumerable<Wait>>>[] subFunctions, [CallerMemberName] string callerName = "")
        {
            var result = new AllFunctionsWait
            {
                WaitingFunctions = new FunctionWait[subFunctions.Length],
                EventIdentifier = eventIdentifier,
                InitiatedByFunction = callerName,
            };
            for (int i = 0; i < subFunctions.Length; i++)
            {
                var currentFunction = subFunctions[i];
                var currentFuncResult = await Function("Function Name", currentFunction);
                result.WaitingFunctions[i] = currentFuncResult;
            }
            return result;
        }

        protected Expression<Func<IAsyncEnumerable<Wait>>>[] FunctionGroup(params Expression<Func<IAsyncEnumerable<Wait>>>[] subFunctions) => subFunctions;
        protected async Task<AnyFunctionWait> AnyFunction
            (string eventIdentifier, Expression<Func<IAsyncEnumerable<Wait>>>[] subFunctions, [CallerMemberName] string callerName = "")
        {
            var result = new AnyFunctionWait
            {
                WaitingFunctions = new FunctionWait[subFunctions.Length],
                EventIdentifier = eventIdentifier,
                InitiatedByFunction = callerName
            };
            for (int i = 0; i < subFunctions.Length; i++)
            {
                var currentFunction = subFunctions[i];
                var currentFuncResult = await Function("Function Name", currentFunction);
                result.WaitingFunctions[i] = currentFuncResult;
            }
            return result;
        }
    }

}
