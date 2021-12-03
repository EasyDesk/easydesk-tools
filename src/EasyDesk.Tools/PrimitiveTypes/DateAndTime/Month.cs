namespace EasyDesk.Tools.PrimitiveTypes.DateAndTime
{
    public enum Month
    {
        January = 1,
        February,
        March,
        April,
        May,
        June,
        July,
        August,
        September,
        October,
        November,
        December
    }

    public static class MonthExtensions
    {
        public static int NumberOfDays(this Month month, bool isLeapYear)
        {
            return month switch
            {
                Month.February => isLeapYear ? 29 : 28,
                Month.April or Month.June or Month.September or Month.November => 30,
                _ => 31
            };
        }
    }
}
