namespace EasyDesk.Tools.PrimitiveTypes.Intervals
{
    public record IntervalBound<T>
    {
        private IntervalBound(T instant, bool closed)
        {
            Instant = instant;
            IsClosed = closed;
        }

        public T Instant { get; }

        public bool IsClosed { get; }

        public bool IsOpen => !IsClosed;

        public static IntervalBound<T> Create(T instant, bool closed = true) => new(instant, closed);

        public static IntervalBound<T> Closed(T instant) => Create(instant, closed: true);

        public static IntervalBound<T> Open(T instant) => Create(instant, closed: false);
    }
}
