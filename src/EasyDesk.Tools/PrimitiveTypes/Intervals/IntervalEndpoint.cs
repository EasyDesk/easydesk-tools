using EasyDesk.Tools.Options;
using System;
using static EasyDesk.Tools.Options.OptionImports;

namespace EasyDesk.Tools.PrimitiveTypes.Intervals
{
    public record IntervalEndpoint<T>
        where T : IComparable<T>
    {
        private IntervalEndpoint()
        {
            Bound = None;
        }

        private IntervalEndpoint(IntervalBound<T> bound)
        {
            Bound = Some(bound);
        }

        public Option<IntervalBound<T>> Bound { get; }

        public bool IsInfinite => Bound.IsAbsent;

        public bool IsFinite => !IsInfinite;

        public static IntervalEndpoint<T> BoundedClosed(T instant) => Bounded(IntervalBound<T>.Closed(instant));

        public static IntervalEndpoint<T> BoundedOpen(T instant) => Bounded(IntervalBound<T>.Open(instant));

        public static IntervalEndpoint<T> Bounded(IntervalBound<T> bound) => new(bound);

        public static IntervalEndpoint<T> Unbounded { get; } = new();
    }
}
