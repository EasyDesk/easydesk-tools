using EasyDesk.Testing.Utils;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using IntegerEndpoint = EasyDesk.Tools.PrimitiveTypes.Intervals.IntervalEndpoint<int>;
using IntegerInterval = EasyDesk.Tools.PrimitiveTypes.Intervals.Interval<int, int,
    EasyDesk.Tools.PrimitiveTypes.Intervals.Metrics.IntegerMetric>;

namespace EasyDesk.Tools.UnitTests.PrimitiveTypes.Intervals
{
    public class IntervalTests
    {
        [Theory]
        [MemberData(nameof(StrictlyBetweenData))]
        public void Contains_ShouldReturnTrue_ForValuesThatAreStrictlyBetweenTheEndpoints(
            IntegerEndpoint lower, IntegerEndpoint upper, int value)
        {
            var interval = IntegerInterval.FromEndpoints(lower, upper);

            interval.Contains(value).ShouldBeTrue();
        }

        public static IEnumerable<object[]> StrictlyBetweenData()
        {
            var lower = 0;
            var upper = 10;
            var value = 5;
            return Matrix
                .Axis(IntervalUtils.AllEndpointTypes(lower))
                .Axis(IntervalUtils.AllEndpointTypes(upper))
                .GeneratedValue(_ => value)
                .Build();
        }

        [Theory]
        [MemberData(nameof(AtClosedBoundsData))]
        public void Contains_ShouldReturnTrue_ForValuesCoincidingWithClosedBounds(
            IntegerEndpoint lower, IntegerEndpoint upper, int value)
        {
            var interval = IntegerInterval.FromEndpoints(lower, upper);

            interval.Contains(value).ShouldBeTrue();
        }

        public static IEnumerable<object[]> AtClosedBoundsData()
        {
            var lower = 0;
            var upper = 10;
            var lowerClosed = Matrix
                .Fixed(IntegerEndpoint.BoundedClosed(lower))
                .Axis(IntervalUtils.AllEndpointTypes(upper))
                .Fixed(lower)
                .Build();
            var upperClosed = Matrix
                .Axis(IntervalUtils.AllEndpointTypes(lower))
                .Fixed(IntegerEndpoint.BoundedClosed(upper))
                .Fixed(upper)
                .Build();
            return lowerClosed.Concat(upperClosed);
        }

        [Theory]
        [MemberData(nameof(StricltyOutsideData))]
        public void Contains_ShouldReturnFalse_ForValuesThatAreStrictlyOutsideTheEndpoints(
            IntegerEndpoint lower, IntegerEndpoint upper, int value)
        {
            var interval = IntegerInterval.FromEndpoints(lower, upper);

            interval.Contains(value).ShouldBeFalse();
        }

        public static IEnumerable<object[]> StricltyOutsideData()
        {
            var lower = 0;
            var upper = 10;
            var delta = 2;
            var beforeLower = lower - delta;
            var afterUpper = upper + delta;
            var lowerBounded = Matrix
                .Axis(IntervalUtils.AllBoundedEndpointTypes(lower))
                .Axis(IntervalUtils.AllEndpointTypes(upper))
                .Fixed(beforeLower)
                .Build();
            var upperBounded = Matrix
                .Axis(IntervalUtils.AllEndpointTypes(lower))
                .Axis(IntervalUtils.AllBoundedEndpointTypes(upper))
                .Fixed(afterUpper)
                .Build();
            return lowerBounded.Concat(upperBounded);
        }
    }
}
