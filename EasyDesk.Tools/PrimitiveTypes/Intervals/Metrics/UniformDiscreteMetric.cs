namespace EasyDesk.Core.PrimitiveTypes.Intervals.Metrics
{
    public abstract class UniformDiscreteMetric<T, D> : UniformMetric<T, D>
    {
        public override D NullOffset => Multiply(UnaryOffset, 0);

        protected abstract D UnaryOffset { get; }

        public override IntervalBound<T> AddOffset(IntervalBound<T> start, D offset, bool closed) =>
            ApplyOffset(start, offset, closed, multiplier: 1);

        public override IntervalBound<T> SubtractOffset(IntervalBound<T> start, D offset, bool closed) =>
            ApplyOffset(start, offset, closed, multiplier: -1);

        private IntervalBound<T> ApplyOffset(IntervalBound<T> start, D offset, bool closed, int multiplier)
        {
            var factor = OffsetCorrectionFactor(start.IsClosed, closed) * multiplier;
            var correction = Multiply(UnaryOffset, factor);
            var corrected = Add(start.Instant, correction);
            var offsetted = Add(corrected, Multiply(offset, multiplier));
            return IntervalBound<T>.Create(offsetted, closed);
        }

        private int OffsetCorrectionFactor(bool startClosed, bool endClosed) =>
            AsInt(startClosed) - AsInt(endClosed);

        private int AsInt(bool boolean) => boolean ? 1 : 0;

        public override D OffsetBetween(IntervalBound<T> from, IntervalBound<T> to)
        {
            var correctionFactor = AsInt(from.IsClosed) + AsInt(to.IsClosed) - 1;
            var correction = Multiply(UnaryOffset, correctionFactor);
            return Distance(from.Instant, Add(to.Instant, correction));
        }

        protected abstract D Multiply(D offset, int factor);

        protected override D Negate(D offset) => Multiply(offset, -1);
    }
}
