using System;
using System.Linq;

namespace Airslip.Common.Types.Extensions
{
    public static class DateTimeExtensions
    {
        public static long ToUnixTimeMilliseconds(this DateTime value)
        {
            return new DateTimeOffset(value).ToUnixTimeMilliseconds();
        }

        public static bool IsBetweenTwoDates(this long dt, long start, long end)
        {
            return dt >= start && dt <= end;
        }

        public static string ToIso8601(this DateTime datetime)
        {
            return DateTime.SpecifyKind(datetime, DateTimeKind.Utc).ToString("O");
        }

        public static string ToIso8601(this long datetime)
        {
            DateTimeOffset date = DateTimeOffset.FromUnixTimeMilliseconds(datetime);
            return DateTime.SpecifyKind(date.DateTime, DateTimeKind.Utc).ToString("O");
        }

        public static long? GetEarliestDate(params DateTimeOffset?[] dates)
        {
            return dates.Min()?.ToUnixTimeMilliseconds();
        }

        public static int GetMonthsBetweenDates(DateTimeOffset startDate, DateTimeOffset endDate)
        {
            return (int) Math.Round(endDate.Subtract(startDate).Days / (365.25 / 12));
        }
    }
}