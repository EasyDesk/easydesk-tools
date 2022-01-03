namespace EasyDesk.Tools.PrimitiveTypes.Intervals.Metrics;

public class IntegerMetric : UniformDiscreteMetric<int, int>
{
    protected override int UnaryOffset => 1;

    protected override int Multiply(int offset, int factor) => offset * factor;

    protected override int Add(int instant, int offset) => instant + offset;

    protected override int Distance(int from, int to) => to - from;
}
