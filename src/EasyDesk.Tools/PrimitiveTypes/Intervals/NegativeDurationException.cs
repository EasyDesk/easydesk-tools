using System;

namespace EasyDesk.Tools.PrimitiveTypes.Intervals
{
    public class NegativeDurationException : Exception
    {
        public NegativeDurationException()
            : base("Interval duration cannot be negative")
        {
        }
    }
}
