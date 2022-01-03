using EasyDesk.Tools.PrimitiveTypes.Intervals.Metrics;
using System;
using System.Globalization;

namespace EasyDesk.Tools.PrimitiveTypes.DateAndTime;

public record TimeOfDay : IComparable<TimeOfDay>, IFormattable
{
    private TimeOfDay(TimeSpan timeSpan)
    {
        if (timeSpan < TimeSpan.Zero)
        {
            throw new ArgumentException("Time of day cannot be negative", nameof(timeSpan));
        }

        if (timeSpan >= TimeSpan.FromDays(1))
        {
            throw new ArgumentException("Time of day cannot exceed 24 hours", nameof(timeSpan));
        }

        AsTimeSpan = timeSpan;
    }

    public static TimeOfDay StartOfDay => FromTimeSpan(TimeSpan.Zero);

    public static TimeOfDay FromTimeSpan(TimeSpan timeSpan) => new(timeSpan);

    public static TimeOfDay Parse(string timeOfDay) => FromTimeSpan(TimeSpan.Parse(timeOfDay, CultureInfo.InvariantCulture));

    public TimeSpan AsTimeSpan { get; }

    public int CompareTo(TimeOfDay other) => AsTimeSpan.CompareTo(other.AsTimeSpan);

    public static TimeOffset operator -(TimeOfDay left, TimeOfDay right) => TimeOffset.FromTimeSpan(left.AsTimeSpan - right.AsTimeSpan);

    public static TimeOfDay operator +(TimeOfDay left, TimeOffset right) => FromTimeSpan(left.AsTimeSpan + right.AsTimeSpan);

    public static TimeOfDay operator -(TimeOfDay left, TimeOffset right) => FromTimeSpan(left.AsTimeSpan - right.AsTimeSpan);

    public override string ToString() => ToString(DateTimeFormats.Time.Long);

    public string ToString(string format) => ToString(format, CultureInfo.InvariantCulture);

    public string ToString(string format, IFormatProvider formatProvider) => AsTimeSpan.ToString(format, formatProvider);

    public static bool operator <(TimeOfDay left, TimeOfDay right) => left.CompareTo(right) < 0;

    public static bool operator <=(TimeOfDay left, TimeOfDay right) => left.CompareTo(right) <= 0;

    public static bool operator >(TimeOfDay left, TimeOfDay right) => left.CompareTo(right) > 0;

    public static bool operator >=(TimeOfDay left, TimeOfDay right) => left.CompareTo(right) >= 0;
}

public class TimeOfDayMetric : UniformContinuousMetric<TimeOfDay, TimeOffset>
{
    public override TimeOffset NullOffset => TimeOffset.Zero;

    protected override TimeOfDay Add(TimeOfDay instant, TimeOffset offset) => instant + offset;

    protected override TimeOffset Distance(TimeOfDay from, TimeOfDay to) => to - from;

    protected override TimeOffset Negate(TimeOffset offset) => -offset;
}
