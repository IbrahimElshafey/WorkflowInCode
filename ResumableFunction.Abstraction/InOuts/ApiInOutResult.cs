﻿namespace ResumableFunction.Abstraction.InOuts
{
    public class ApiInOutResult : IEventData
    {
        public virtual string Url { get; }
        public object Args { get; set; }
        public object Result { get; set; }

        public virtual string EventProviderName { get; set; }
    }


}
