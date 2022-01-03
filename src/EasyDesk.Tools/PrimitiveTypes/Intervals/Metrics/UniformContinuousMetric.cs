using System;

namespace EasyDesk.Tools.PrimitiveTypes.Intervals.Metrics;

public abstract class UniformContinuousMetric<T, D> : UniformMetric<T, D>
    where T : IComparable<T>
    where D : IComparable<D>
{
    public override IntervalBound<T> AddOffset(IntervalBound<T> start, D offset, bool closed) =>
        IntervalBound<T>.Create(Add(start.Instant, offset), closed);

    public override IntervalBound<T> SubtractOffset(IntervalBound<T> start, D offset, bool closed) =>
        AddOffset(start, Negate(offset), closed);

    public override D OffsetBetween(IntervalBound<T> from, IntervalBound<T> to) =>
        Distance(from.Instant, to.Instant);
}
