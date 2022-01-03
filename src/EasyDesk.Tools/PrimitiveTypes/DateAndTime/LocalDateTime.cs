using EasyDesk.Tools.PrimitiveTypes.Intervals.Metrics;
using System;
using System.Globalization;

namespace EasyDesk.Tools.PrimitiveTypes.DateAndTime;

public record LocalDateTime : IComparable<LocalDateTime>, IFormattable
{
    private LocalDateTime(DateTime dateTime)
    {
        AsDateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified);
    }

    public static LocalDateTime FromDateTime(DateTime dateTime) => new(dateTime);

    public static LocalDateTime FromDateAndTimeOfDay(Date date, TimeOfDay timeOfDay) =>
        FromDateTime(date.AsDateTime + timeOfDay.AsTimeSpan);

    public static LocalDateTime FromDate(Date date) =>
        FromDateAndTimeOfDay(date, TimeOfDay.StartOfDay);

    public static LocalDateTime Parse(string localDateTime) =>
        FromDateTime(DateTime.Parse(localDateTime, CultureInfo.InvariantCulture));

    public DateTime AsDateTime { get; }

    public Date Date => Date.FromDateTime(AsDateTime);

    public TimeOfDay TimeOfDay => TimeOfDay.FromTimeSpan(AsDateTime.TimeOfDay);

    public int CompareTo(LocalDateTime other) => AsDateTime.CompareTo(other.AsDateTime);

    public static LocalDateTime operator +(LocalDateTime left, TimeOffset right) =>
        FromDateTime(left.AsDateTime + right.AsTimeSpan);

    public static LocalDateTime operator -(LocalDateTime left, TimeOffset right) =>
        FromDateTime(left.AsDateTime - right.AsTimeSpan);

    public static TimeOffset operator -(LocalDateTime left, LocalDateTime right) =>
        TimeOffset.FromTimeSpan(left.AsDateTime - right.AsDateTime);

    public override string ToString() => ToString(DateTimeFormats.RoundTripDateTime);

    public string ToString(string format) => ToString(format, CultureInfo.InvariantCulture);

    public string ToString(string format, IFormatProvider formatProvider) => AsDateTime.ToString(format, formatProvider);

    public static bool operator <(LocalDateTime left, LocalDateTime right) => left.CompareTo(right) < 0;

    public static bool operator <=(LocalDateTime left, LocalDateTime right) => left.CompareTo(right) <= 0;

    public static bool operator >(LocalDateTime left, LocalDateTime right) => left.CompareTo(right) > 0;

    public static bool operator >=(LocalDateTime left, LocalDateTime right) => left.CompareTo(right) >= 0;
}

public class LocalDateTimeMetric : UniformContinuousMetric<LocalDateTime, TimeOffset>
{
    public override TimeOffset NullOffset => TimeOffset.Zero;

    protected override LocalDateTime Add(LocalDateTime instant, TimeOffset offset) => instant + offset;

    protected override TimeOffset Distance(LocalDateTime from, LocalDateTime to) => to - from;

    protected override TimeOffset Negate(TimeOffset offset) => -offset;
}
