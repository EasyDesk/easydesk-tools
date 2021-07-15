using System;

namespace EasyDesk.Core.PrimitiveTypes.Intervals
{
    public class NegativeDurationException : Exception
    {
        public NegativeDurationException()
            : base("Interval duration cannot be negative")
        {
        }
    }
}
