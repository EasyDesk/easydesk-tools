using EasyDesk.Tools.PrimitiveTypes.Intervals;
using EasyDesk.Testing.Utils;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using IntegerEndpoint = EasyDesk.Tools.PrimitiveTypes.Intervals.IntervalEndpoint<int>;
using IntegerExtension = EasyDesk.Tools.PrimitiveTypes.Intervals.IntervalExtension<int>;
using IntegerInterval = EasyDesk.Tools.PrimitiveTypes.Intervals.Interval<int, int,
    EasyDesk.Tools.PrimitiveTypes.Intervals.Metrics.IntegerMetric>;

namespace EasyDesk.Tools.UnitTests.PrimitiveTypes.Intervals
{
    public class IntervalFactoriesTests
    {
        [Fact]
        public void FromStartAndExtension_ShouldReturnAnHalfOpenFiniteInterval_IfExtensionIsFinite()
        {
            var span = 3;
            var extension = IntegerExtension.Finite(span);
            var interval = IntegerInterval.FromStartAndExtension(0, extension);
            interval.LowerEndpoint.ShouldBe(IntegerEndpoint.BoundedClosed(0));
            interval.UpperEndpoint.ShouldBe(IntegerEndpoint.BoundedOpen(span + 1));
            interval.Extension.ShouldBe(extension);
        }

        [Fact]
        public void FromStartAndExtension_ShouldReturnAnInfiniteInterval_IfExtensionIsInfinite()
        {
            var interval = IntegerInterval.FromStartAndExtension(0, IntegerExtension.Infinite);
            interval.LowerEndpoint.ShouldBe(IntegerEndpoint.BoundedClosed(0));
            interval.UpperEndpoint.ShouldBe(IntegerEndpoint.Unbounded);
            interval.Extension.ShouldBe(IntegerExtension.Infinite);
        }

        [Fact]
        public void FromEndAndExtension_ShouldReturnAnHalfOpenFiniteInterval_IfExtensionIsFinite()
        {
            var span = 3;
            var extension = IntegerExtension.Finite(span);
            var interval = IntegerInterval.FromEndAndExtension(0, extension);
            interval.UpperEndpoint.ShouldBe(IntegerEndpoint.BoundedClosed(0));
            interval.LowerEndpoint.ShouldBe(IntegerEndpoint.BoundedOpen(-(span + 1)));
            interval.Extension.ShouldBe(extension);
        }

        [Fact]
        public void FromEndAndExtension_ShouldReturnAnInfiniteInterval_IfExtensionIsInfinite()
        {
            var interval = IntegerInterval.FromEndAndExtension(0, IntegerExtension.Infinite);
            interval.UpperEndpoint.ShouldBe(IntegerEndpoint.BoundedClosed(0));
            interval.LowerEndpoint.ShouldBe(IntegerEndpoint.Unbounded);
            interval.Extension.ShouldBe(IntegerExtension.Infinite);
        }

        [Theory]
        [MemberData(nameof(UnboundedEndpointsData))]
        public void FromEndpoints_ShouldReturnAnInfiniteInterval_IfAnyEndpointIsUnbounded(
            IntegerEndpoint lower, IntegerEndpoint upper)
        {
            var interval = IntegerInterval.FromEndpoints(lower, upper);

            interval.Extension.ShouldBe(IntegerExtension.Infinite);
        }

        public static IEnumerable<object[]> UnboundedEndpointsData()
        {
            var value = 0;
            var endpointTypes = IntervalUtils.AllEndpointTypes(value);
            return Matrix
                .Axis(endpointTypes)
                .Axis(endpointTypes)
                .Filter(t => t.Item1.IsInfinite || t.Item2.IsInfinite)
                .Build();
        }

        [Theory]
        [MemberData(nameof(BoundedEndpointsData))]
        public void FromEndpoints_ShouldReturnAFiniteInterval_IfBothEndpointsAreBounded(
            IntegerEndpoint lower, IntegerEndpoint upper, int expextedExtension)
        {
            var interval = IntegerInterval.FromEndpoints(lower, upper);

            interval.Extension.ShouldBe(IntegerExtension.Finite(expextedExtension));
        }

        public static IEnumerable<object[]> BoundedEndpointsData()
        {
            var lower = 0;
            var upper = 10;
            return Matrix
                .Axis(IntervalUtils.AllBoundedEndpointTypes(lower))
                .Axis(IntervalUtils.AllBoundedEndpointTypes(upper))
                .GeneratedValue(t => upper - lower - 1
                    + Convert.ToInt32(t.Item1.Bound.Value.IsClosed)
                    + Convert.ToInt32(t.Item2.Bound.Value.IsClosed))
                .Build();
        }

        [Theory]
        [MemberData(nameof(OutOfOrderEndpointsData))]
        public void FromEndpoints_ShouldFail_IfEndpointsAreOutOfOrder(
            IntegerEndpoint lower, IntegerEndpoint upper)
        {
            Should.Throw<NegativeDurationException>(() => IntegerInterval.FromEndpoints(lower, upper));
        }

        public static IEnumerable<object[]> OutOfOrderEndpointsData()
        {
            var overlappingButOneIsOpen = Matrix
                .Axis(IntervalUtils.AllBoundedEndpointTypes(10))
                .Axis(IntervalUtils.AllBoundedEndpointTypes(10))
                .Filter(t => t.Item1.Bound.Value.IsOpen || t.Item2.Bound.Value.IsOpen)
                .Build();
            var outOfOrder = Matrix
                .Axis(IntervalUtils.AllBoundedEndpointTypes(10))
                .Axis(IntervalUtils.AllBoundedEndpointTypes(0))
                .Build();
            return overlappingButOneIsOpen.Concat(outOfOrder);
        }
    }
}
