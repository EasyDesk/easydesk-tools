using System;
using System.Globalization;

namespace EasyDesk.Tools.PrimitiveTypes.DateAndTime
{
    public record TimeOffset : IComparable<TimeOffset>, IFormattable
    {
        private TimeOffset(TimeSpan timeSpan)
        {
            AsTimeSpan = timeSpan;
        }

        #region Factories
        public static TimeOffset Zero => FromTimeSpan(TimeSpan.Zero);

        public static TimeOffset FromTimeSpan(TimeSpan timeSpan) => new(timeSpan);

        public static TimeOffset FromTimeOfDay(TimeOfDay timeOfDay) => FromTimeSpan(timeOfDay.AsTimeSpan);

        public static TimeOffset FromDays(double days) => FromTimeSpan(TimeSpan.FromDays(days));

        public static TimeOffset FromHours(double hours) => FromTimeSpan(TimeSpan.FromHours(hours));

        public static TimeOffset FromMinutes(double minutes) => FromTimeSpan(TimeSpan.FromMinutes(minutes));

        public static TimeOffset FromSeconds(double seconds) => FromTimeSpan(TimeSpan.FromSeconds(seconds));

        public static TimeOffset FromMilliseconds(double ms) => FromTimeSpan(TimeSpan.FromMilliseconds(ms));


        #endregion

        public TimeSpan AsTimeSpan { get; }

        public int CompareTo(TimeOffset other) => AsTimeSpan.CompareTo(other.AsTimeSpan);

        public static TimeOffset operator +(TimeOffset left, TimeOffset right) => FromTimeSpan(left.AsTimeSpan + right.AsTimeSpan);

        public static TimeOffset operator -(TimeOffset left, TimeOffset right) => FromTimeSpan(left.AsTimeSpan - right.AsTimeSpan);

        public static TimeOffset operator -(TimeOffset timeOffset) => FromTimeSpan(-timeOffset.AsTimeSpan);

        #region Common operators
        public override string ToString() => ToString(TimeFormats.Invariant);

        public string ToString(string format) => ToString(format, CultureInfo.InvariantCulture);

        public string ToString(string format, IFormatProvider formatProvider) => AsTimeSpan.ToString(format, formatProvider);

        public static bool operator <(TimeOffset left, TimeOffset right) => left.CompareTo(right) < 0;

        public static bool operator <=(TimeOffset left, TimeOffset right) => left.CompareTo(right) <= 0;

        public static bool operator >(TimeOffset left, TimeOffset right) => left.CompareTo(right) > 0;

        public static bool operator >=(TimeOffset left, TimeOffset right) => left.CompareTo(right) >= 0;
        #endregion
    }
}
