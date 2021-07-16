using EasyDesk.Tools.Options;
using EasyDesk.Tools.PrimitiveTypes.DateAndTime;
using System.Collections.Generic;
using System.Linq;
using static EasyDesk.Tools.Options.OptionImports;

namespace EasyDesk.Tools
{
    public struct TimeInterval
    {
        public TimeOfDay StartTime { get; }

        public TimeOfDay EndTime { get; }

        public TimeInterval(TimeOfDay start, TimeOfDay end)
        {
            StartTime = start;
            EndTime = end;
        }

        public static implicit operator TimeInterval((TimeOfDay, TimeOfDay) interval) => new(interval.Item1, interval.Item2);
    }

    public struct DateTimeInterval
    {
        public Timestamp Start { get; }

        public Timestamp End { get; }

        public DateTimeInterval(Timestamp start, Timestamp end)
        {
            Start = start;
            End = end;
        }

        public static implicit operator DateTimeInterval((Timestamp, Timestamp) interval) => new(interval.Item1, interval.Item2);
    }

    public struct DateInterval
    {
        public Date StartDate { get; }

        public Date EndDate { get; }

        public DateInterval(Date start, Date end)
        {
            StartDate = start;
            EndDate = end;
        }

        public static implicit operator DateInterval((Date, Date) interval) => new(interval.Item1, interval.Item2);
    }

    public static class DateTimeUtils
    {
        public static Timestamp Max(Timestamp a, Timestamp b) => a > b ? a : b;

        public static Timestamp Min(Timestamp a, Timestamp b) => a < b ? a : b;
    }

    public static class TimeIntervalUtils
    {
        public static bool Contains(this TimeInterval interval, TimeOfDay time) =>
            interval.StartTime <= time && interval.EndTime >= time;

        public static Option<TimeInterval> Intersect(this TimeInterval interval, TimeInterval other)
        {
            var start = Max(interval.StartTime, other.StartTime);
            var end = Min(interval.EndTime, other.EndTime);

            return start < end ? Some<TimeInterval>((start, end)) : None;
        }

        public static TimeOfDay Min(TimeOfDay a, TimeOfDay b) => a < b ? a : b;

        public static TimeOfDay Max(TimeOfDay a, TimeOfDay b) => a > b ? a : b;
    }

    public static class DateIntervalUtils
    {
        public static bool Contains(this DateInterval interval, Date date) =>
            (interval.StartDate is null || interval.StartDate <= date) &&
            (interval.EndDate is null|| interval.EndDate >= date);

        public static Option<DateInterval> Intersect(this DateInterval interval, DateInterval other)
        {
            var start = Max(interval.StartDate, other.StartDate);
            var end = Min(interval.EndDate, other.EndDate);

            var orderIsCorrect = (
                from s in start.AsOption()
                from e in end.AsOption()
                select s <= e).OrElse(true);

            return orderIsCorrect ? Some<DateInterval>((start, end)) : None;
        }

        public static IEnumerable<DateInterval> Complementary(this DateInterval interval)
        {
            var left = interval.StartDate
                .AsOption()
                .Map<Date, DateInterval>(d => (null, d - 1));
            var right = interval.EndDate
                .AsOption()
                .Map<Date, DateInterval>(d => (d + 1, null));

            if (left.IsPresent)
            {
                yield return left.Value;
            }
            if (right.IsPresent)
            {
                yield return right.Value;
            }
        }

        public static IEnumerable<DateInterval> Subtract(this DateInterval interval, DateInterval other)
        {
            return other.Complementary().SelectMany(i => interval.Intersect(i));
        }

        public static Date Min(Date a, Date b)
        {
            if (a is null)
            {
                return b;
            }
            else if (b is null)
            {
                return a;
            }
            return a < b ? a : b;
        }

        public static Date Max(Date a, Date b)
        {
            if (a is null)
            {
                return b;
            }
            else if (b is null)
            {
                return a;
            }
            return a > b ? a : b;
        }
    }
}
