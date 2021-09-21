using System;
using System.Globalization;

namespace EasyDesk.Tools.PrimitiveTypes.DateAndTime
{
    public record Duration : IComparable<Duration>, IFormattable
    {
        private Duration(TimeSpan timeSpan)
        {
            if (timeSpan <= TimeSpan.Zero)
            {
                throw new ArgumentException("Durations must be positive", nameof(timeSpan));
            }

            AsTimeSpan = timeSpan;
        }

        #region Factories
        public static Duration FromTimeSpan(TimeSpan timeSpan) => new(timeSpan);

        public static Duration Parse(string duration) => FromTimeSpan(TimeSpan.Parse(duration, CultureInfo.InvariantCulture));

        public static Duration FromDays(double days) => FromTimeSpan(TimeSpan.FromDays(days));

        public static Duration FromHours(double hours) => FromTimeSpan(TimeSpan.FromHours(hours));

        public static Duration FromMinutes(double minutes) => FromTimeSpan(TimeSpan.FromMinutes(minutes));

        public static Duration FromSeconds(double seconds) => FromTimeSpan(TimeSpan.FromSeconds(seconds));

        public static Duration FromMilliseconds(double ms) => FromTimeSpan(TimeSpan.FromMilliseconds(ms));
        #endregion

        public TimeSpan AsTimeSpan { get; }

        public TimeOffset AsTimeOffset => TimeOffset.FromTimeSpan(AsTimeSpan);

        public int CompareTo(Duration other) => AsTimeSpan.CompareTo(other.AsTimeSpan);

        #region Common operators
        public override string ToString() => ToString(TimeFormats.Invariant);

        public string ToString(string format) => ToString(format, CultureInfo.InvariantCulture);

        public string ToString(string format, IFormatProvider formatProvider) => AsTimeSpan.ToString(format, formatProvider);

        public static bool operator <(Duration left, Duration right) => left.CompareTo(right) < 0;

        public static bool operator >(Duration left, Duration right) => left.CompareTo(right) > 0;

        public static bool operator <=(Duration left, Duration right) => left.CompareTo(right) <= 0;

        public static bool operator >=(Duration left, Duration right) => left.CompareTo(right) >= 0;
        #endregion
    }
}
