﻿using EasyDesk.Core.PrimitiveTypes.Intervals.Metrics;
using System;
using System.Globalization;

namespace EasyDesk.Core.PrimitiveTypes.DateAndTime
{
    public record Date : IComparable<Date>, IFormattable
    {
        private Date(DateTime date)
        {
            AsDateTime = DateTime.SpecifyKind(date, DateTimeKind.Unspecified).Date;
        }

        #region Factories
        public static Date FromDateTime(DateTime dateTime) => new(dateTime);

        public static Date Parse(string date) => FromDateTime(DateTime.Parse(date, CultureInfo.InvariantCulture));
        #endregion

        public DateTime AsDateTime { get; }

        public DayOfWeek DayOfWeek => AsDateTime.DayOfWeek;

        public int CompareTo(Date other) => AsDateTime.CompareTo(other.AsDateTime);

        public static Date operator +(Date left, int right) => FromDateTime(left.AsDateTime + TimeSpan.FromDays(right));

        public static Date operator -(Date left, int right) => FromDateTime(left.AsDateTime - TimeSpan.FromDays(right));

        public static int operator -(Date left, Date right) => (left.AsDateTime - right.AsDateTime).Days;

        #region Common operators
        public override string ToString() => ToString(DateTimeFormats.Date.Short);

        public string ToString(string format) => ToString(format, CultureInfo.InvariantCulture);

        public string ToString(string format, IFormatProvider formatProvider) => AsDateTime.ToString(format, formatProvider);

        public static bool operator <(Date left, Date right) => left.CompareTo(right) < 0;

        public static bool operator <=(Date left, Date right) => left.CompareTo(right) <= 0;

        public static bool operator >(Date left, Date right) => left.CompareTo(right) > 0;

        public static bool operator >=(Date left, Date right) => left.CompareTo(right) >= 0;
        #endregion
    }

    public class DateMetric : UniformDiscreteMetric<Date, int>
    {
        protected override int UnaryOffset => 1;

        protected override int Multiply(int offset, int factor) => offset * factor;

        protected override Date Add(Date instant, int offset) => instant + offset;

        protected override int Distance(Date from, Date to) => from - to;
    }
}
