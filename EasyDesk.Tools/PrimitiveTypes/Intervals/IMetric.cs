namespace EasyDesk.Core.PrimitiveTypes.Intervals
{
    public interface IMetric<T, D>
    {
        D NullOffset { get; }

        IntervalBound<T> AddOffset(IntervalBound<T> start, D offset, bool closed);

        IntervalBound<T> SubtractOffset(IntervalBound<T> start, D offset, bool closed);

        D OffsetBetween(IntervalBound<T> from, IntervalBound<T> to);
    }
}
