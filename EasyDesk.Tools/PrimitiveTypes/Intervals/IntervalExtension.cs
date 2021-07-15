using EasyDesk.Core.Options;
using static EasyDesk.Core.Options.OptionImports;

namespace EasyDesk.Core.PrimitiveTypes.Intervals
{
    public record IntervalExtension<D>
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
    }
}
