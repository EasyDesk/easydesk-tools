namespace EasyDesk.Tools.PrimitiveTypes.Intervals.Metrics
{
    public abstract class UniformMetric<T, D> : IMetric<T, D>
    {
        public abstract D NullOffset { get; }

        public abstract IntervalBound<T> AddOffset(IntervalBound<T> start, D offset, bool closed);

        public abstract IntervalBound<T> SubtractOffset(IntervalBound<T> start, D offset, bool closed);

        public abstract D OffsetBetween(IntervalBound<T> from, IntervalBound<T> to);

        protected abstract T Add(T instant, D offset);

        protected abstract D Distance(T from, T to);

        protected abstract D Negate(D offset);
    }
}
