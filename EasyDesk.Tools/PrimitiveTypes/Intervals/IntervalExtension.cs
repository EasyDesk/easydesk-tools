using EasyDesk.Tools.Options;
using static EasyDesk.Tools.Options.OptionImports;

namespace EasyDesk.Tools.PrimitiveTypes.Intervals
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
