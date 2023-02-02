﻿using System.Linq.Expressions;
using System.Security.Principal;

namespace ResumableFunction.Abstraction.InOuts
{
    public sealed class AllEventsWait : ManyWaits
    {
        public List<EventWait> MatchedEvents { get; internal set; } = new List<EventWait>();
        public LambdaExpression WhenCountExpression { get; internal set; }

        public AllEventsWait WhenMatchedCount(Expression<Func<int, bool>> matchCountFilter)
        {
            WhenCountExpression = matchCountFilter;
            return this;
        }

        internal void MoveToMatched(EventWait currentWait)
        {
            MatchedEvents.Add(currentWait);
            WaitingEvents.Remove(currentWait);
            CheckIsDone();
        }

        private bool CheckIsDone()
        {
            if (WhenCountExpression is null)
            {
                var required = WaitingEvents.Count(x => x.IsOptional == false);
                Status = required == 0 ? WaitStatus.Completed : Status;
            }
            else
            {
                var matchedCount = MatchedEvents.Count;
                var matchCompiled = (Func<int, bool>)WhenCountExpression.Compile();
                Status = matchCompiled(matchedCount) ? WaitStatus.Completed : Status;
            }
            return Status == WaitStatus.Completed;
        }
    }
}