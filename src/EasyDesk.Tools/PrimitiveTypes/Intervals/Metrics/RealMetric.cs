namespace EasyDesk.Tools.PrimitiveTypes.Intervals.Metrics;

public class RealMetric : UniformContinuousMetric<double, double>
{
    public override double NullOffset => 0;

    protected override double Add(double instant, double offset) => instant + offset;

    protected override double Distance(double from, double to) => to - from;

    protected override double Negate(double offset) => -offset;
}
