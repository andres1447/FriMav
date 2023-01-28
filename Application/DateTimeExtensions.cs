using System;

namespace FriMav.Application
{
    public static class DateTimeExtensions
    {
        public static DateTime FirstDayOfWeek(this DateTime date)
        {
            int diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
            return date.AddDays(-1 * diff).Date;
        }

        public static DateTime LastDayOfWeek(this DateTime date) => FirstDayOfWeek(date).AddDays(6);

        public static DateTime EndOfDay(this DateTime date) => date.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

        public static bool IsLastDayOfMonth(this DateTime date) => date.AddDays(1).Month != date.Month;
    }
}
