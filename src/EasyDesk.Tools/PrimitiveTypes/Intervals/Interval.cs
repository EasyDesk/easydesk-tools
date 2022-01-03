using EasyDesk.Tools.Options;
using System;
using System.Linq;
using static EasyDesk.Tools.Functions;

namespace EasyDesk.Tools.PrimitiveTypes.Intervals;

public record Interval<T, D, M>
    where T : IComparable<T>
    where D : IComparable<D>
    where M : IMetric<T, D>, new()
{
    private Interval(IntervalEndpoint<T> lowerEndpoint, IntervalEndpoint<T> upperEndpoint, IntervalExtension<D> extension)
    {
        LowerEndpoint = lowerEndpoint;
        UpperEndpoint = upperEndpoint;
        Extension = extension;
    }

    public IntervalEndpoint<T> LowerEndpoint { get; }

    public IntervalEndpoint<T> UpperEndpoint { get; }

    public IntervalExtension<D> Extension { get; }

    public bool Contains(T instant)
    {
        return InstantIsOnSideOfEndpoint(instant, GreaterThan, LowerEndpoint)
            && InstantIsOnSideOfEndpoint(instant, LessThan, UpperEndpoint);
    }

    private bool InstantIsOnSideOfEndpoint(T instant, Func<int, bool> correctSide, IntervalEndpoint<T> endpoint)
    {
        return endpoint.Bound
            .Map(bound =>
            {
                var side = instant.CompareTo(bound.Instant);
                return correctSide(side) || (bound.IsClosed && side == 0);
            })
            .OrElse(true);
    }

    public bool Intersects(Interval<T, D, M> other)
    {
        var rightmostLowerEndpoint = InnermostEndpoint(LowerEndpoint, other.LowerEndpoint, GreaterThan);
        var leftmostUpperEndpoint = InnermostEndpoint(UpperEndpoint, other.UpperEndpoint, LessThan);

        var innermostBoundsAreOrdered =
            from lower in rightmostLowerEndpoint.Bound
            from upper in leftmostUpperEndpoint.Bound
            select BoundsAreCorrectlyOrdered(lower, upper);

        return innermostBoundsAreOrdered | true;
    }

    private IntervalEndpoint<T> InnermostEndpoint(IntervalEndpoint<T> a, IntervalEndpoint<T> b, Func<int, bool> correctSide)
    {
        if (a.IsInfinite)
        {
            return b;
        }

        if (b.IsInfinite)
        {
            return a;
        }

        var boundA = a.Bound.Value;
        var boundB = b.Bound.Value;
        var sideOfA = boundA.Instant.CompareTo(boundB.Instant);
        return correctSide(sideOfA) || (sideOfA == 0 && boundB.IsClosed) ? a : b;
    }

    private bool BoundsAreCorrectlyOrdered(IntervalBound<T> lower, IntervalBound<T> upper)
    {
        var compareResult = lower.Instant.CompareTo(upper.Instant);
        return compareResult <= 0 || (compareResult == 0 && lower.IsClosed && upper.IsClosed);
    }

    public static Interval<T, D, M> FromStartAndExtension(T start, IntervalExtension<D> extension) =>
        HalfOpen(start, extension, instantIsLowerBound: true);

    public static Interval<T, D, M> FromEndAndExtension(T end, IntervalExtension<D> extension) =>
        HalfOpen(end, extension, instantIsLowerBound: false);

    private static Interval<T, D, M> HalfOpen(T instant, IntervalExtension<D> extension, bool instantIsLowerBound = true)
    {
        var metric = new M();
        var bound = IntervalBound<T>.Closed(instant);
        var endpoint = IntervalEndpoint<T>.Bounded(bound);
        var otherEndpoint = extension.Duration
            .Map(d => instantIsLowerBound
                ? metric.AddOffset(bound, d, closed: false)
                : metric.SubtractOffset(bound, d, closed: false))
            .Map(IntervalEndpoint<T>.Bounded)
            .OrElseGet(() => IntervalEndpoint<T>.Unbounded);

        var lowerEndpoint = instantIsLowerBound ? endpoint : otherEndpoint;
        var upperEndpoint = instantIsLowerBound ? otherEndpoint : endpoint;
        return new(lowerEndpoint, upperEndpoint, extension);
    }

    public static Interval<T, D, M> FromEndpoints(IntervalEndpoint<T> lowerBound, IntervalEndpoint<T> upperBound)
    {
        var bounds =
            from s in lowerBound.Bound
            from e in upperBound.Bound
            select (lower: s, upper: e);

        var extension = bounds
            .IfPresent(b => RequireOrderedBounds(b.lower, b.upper))
            .Map(b => IntervalExtension<D>.Finite(new M().OffsetBetween(b.lower, b.upper)));

        return new(lowerBound, upperBound, extension | IntervalExtension<D>.Infinite);
    }

    private static void RequireOrderedBounds(IntervalBound<T> lowerBound, IntervalBound<T> upperBound)
    {
        if (!BoundsAreOrdered(lowerBound, upperBound))
        {
            throw new NegativeDurationException();
        }
    }

    private static bool BoundsAreOrdered(IntervalBound<T> lowerBound, IntervalBound<T> upperBound)
    {
        if (lowerBound.Instant.Equals(upperBound.Instant))
        {
            return lowerBound.IsClosed && upperBound.IsClosed;
        }
        return lowerBound.Instant.IsLessThan(upperBound.Instant);
    }
}
