using EasyDesk.Tools.Options;
using System;
using static EasyDesk.Tools.Options.OptionImports;

namespace EasyDesk.Tools.PrimitiveTypes.Intervals
{
    public record IntervalExtension<D> : IComparable<IntervalExtension<D>>
        where D : IComparable<D>
    {
        private IntervalExtension(D duration)
        {
            Duration = Some(duration);
        }

        private IntervalExtension()
        {
            Duration = None;
        }

        public Option<D> Duration { get; }

        public bool IsInfinite => Duration.IsAbsent;

        public static IntervalExtension<D> Finite(D duration) => new(duration);

        public static IntervalExtension<D> Infinite { get; } = new();

        public int CompareTo(IntervalExtension<D> other) => (IsInfinite, other.IsInfinite) switch
        {
            (true, true) => 0,
            (false, true) => -1,
            (true, false) => 1,
            (false, false) => Duration.Value.CompareTo(other.Duration.Value)
        };
    }
}
