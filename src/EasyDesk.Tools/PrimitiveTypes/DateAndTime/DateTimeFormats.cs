namespace EasyDesk.Tools.PrimitiveTypes.DateAndTime
{
    public static class DateTimeFormats
    {
        public static class Date
        {
            public const string Short = "d";

            public const string Long = "D";
        }

        public static class FullDateTime
        {
            public const string ShortTime = "f";

            public const string LongTime = "F";
        }

        public static class GeneralDateTime
        {
            public const string ShortTime = "g";

            public const string LongTime = "G";
        }

        public static class Time
        {
            public const string Short = "t";

            public const string Long = "T";
        }

        public static class Universal
        {
            public const string Sortable = "u";

            public const string Full = "U";
        }

        public const string YearMonth = "y";

        public const string MonthDay = "m";

        public const string RoundTripDateTime = "o";

        public const string Rfc1123 = "r";

        public const string SortableDateTime = "s";
    }
}
