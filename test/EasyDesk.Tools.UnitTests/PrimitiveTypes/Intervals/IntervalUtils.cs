using EasyDesk.Tools.PrimitiveTypes.Intervals;
using System.Collections.Generic;

namespace EasyDesk.Tools.UnitTests.PrimitiveTypes.Intervals
{
    public static class IntervalUtils
    {
        public static IEnumerable<IntervalEndpoint<T>> AllEndpointTypes<T>(T value)
        {
            yield return IntervalEndpoint<T>.BoundedOpen(value);
            yield return IntervalEndpoint<T>.BoundedClosed(value);
            yield return IntervalEndpoint<T>.Unbounded;
        }

        public static IEnumerable<IntervalEndpoint<T>> AllBoundedEndpointTypes<T>(T value)
        {
            yield return IntervalEndpoint<T>.BoundedOpen(value);
            yield return IntervalEndpoint<T>.BoundedClosed(value);
        }
    }
}
