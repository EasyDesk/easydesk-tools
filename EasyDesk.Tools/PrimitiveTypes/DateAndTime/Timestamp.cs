using EasyDesk.Core.PrimitiveTypes.Intervals.Metrics;
using System;
using System.Globalization;

namespace EasyDesk.Core.PrimitiveTypes.DateAndTime
{
    public record Timestamp : IComparable<Timestamp>, IFormattable
    {
        private Timestamp(DateTime dateTime)
        {
            if (dateTime.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException("The DateTimeKind must be UTC", nameof(dateTime));
            }

            AsDateTime = dateTime;
        }

        #region Factories
        public static Timestamp FromUtcDateTime(DateTime dateTime) => new(dateTime);

        public static Timestamp Now => FromUtcDateTime(DateTime.UtcNow);

        public static Timestamp Parse(string timestamp) =>
            FromUtcDateTime(DateTime.Parse(timestamp, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal));
        #endregion

        public DateTime AsDateTime { get; }

        public int CompareTo(Timestamp other) => AsDateTime.CompareTo(other.AsDateTime);

        public static Timestamp operator +(Timestamp left, TimeOffset right) => FromUtcDateTime(left.AsDateTime + right.AsTimeSpan);

        public static Timestamp operator -(Timestamp left, TimeOffset right) => FromUtcDateTime(left.AsDateTime - right.AsTimeSpan);

        public static TimeOffset operator -(Timestamp left, Timestamp right) => TimeOffset.FromTimeSpan(left.AsDateTime - right.AsDateTime);

        #region Common operators
        public override string ToString() => ToString(DateTimeFormats.RoundTripDateTime);

        public string ToString(string format) => ToString(format, CultureInfo.InvariantCulture);

        public string ToString(string format, IFormatProvider formatProvider) => AsDateTime.ToString(format, formatProvider);

        public static bool operator <(Timestamp left, Timestamp right) => left.CompareTo(right) < 0;

        public static bool operator <=(Timestamp left, Timestamp right) => left.CompareTo(right) <= 0;

        public static bool operator >(Timestamp left, Timestamp right) => left.CompareTo(right) > 0;

        public static bool operator >=(Timestamp left, Timestamp right) => left.CompareTo(right) >= 0;
        #endregion
    }

    public class TimestampMetric : UniformContinuousMetric<Timestamp, TimeOffset>
    {
        public override TimeOffset NullOffset => TimeOffset.Zero;

        protected override Timestamp Add(Timestamp instant, TimeOffset offset) => instant + offset;

        protected override TimeOffset Distance(Timestamp from, Timestamp to) => to - from;

        protected override TimeOffset Negate(TimeOffset offset) => -offset;
    }
}
