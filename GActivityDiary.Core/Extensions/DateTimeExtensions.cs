using GActivityDiary.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GActivityDiary.Core.Extensions
{
    /// <summary>
    /// <see cref="DateTime"/> extensions.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Get a quarter.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int GetQuarter(this DateTime date)
        {
            return date.Month switch
            {
                >= 1 and <= 3 => 1,
                >= 4 and <= 6 => 2,
                >= 7 and <= 9 => 3,
                _ => 4
            };
        }

        /// <summary>
        /// Get the <see cref="DateTimeInterval"/> of the day.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTimeInterval GetDayInterval(this DateTime dateTime)
        {
            return new(dateTime);
        }

        /// <summary>
        /// Get the <see cref="DateTimeInterval"/> of the week that includes the current date and time.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTimeInterval GetWeekInterval(this DateTime dateTime)
        {
            int dayOfWeek = (int)dateTime.DayOfWeek;
            dateTime = dateTime.AddDays(-dayOfWeek + 1);
            DateTime startDateTime = new(dateTime.Year,
                                         dateTime.Month,
                                         dateTime.Day);
            DateTime endDateTime = startDateTime.AddDays(7)
                                                .AddTicks(-1);
            return new(startDateTime, endDateTime);
        }

        /// <summary>
        /// Get the <see cref="DateTimeInterval"/> of the month that includes the current date and time.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTimeInterval GetMonthInterval(this DateTime dateTime)
        {
            DateTime startDateTime = new(dateTime.Year, dateTime.Month, 1);
            DateTime endDateTime = startDateTime.AddMonths(1)
                                                .AddTicks(-1);
            return new(startDateTime, endDateTime);
        }

        /// <summary>
        /// Get the <see cref="DateTimeInterval"/> of the quarter that includes the current date and time.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTimeInterval GetQuarterInterval(this DateTime dateTime)
        {
            int quarter = dateTime.GetQuarter();
            int month = quarter * 3 - 2;
            DateTime startDateTime = new(dateTime.Year, month, 1);
            DateTime endDateTime = startDateTime.AddMonths(3)
                                                .AddTicks(-1);
            return new(startDateTime, endDateTime);
        }

        /// <summary>
        /// Get the <see cref="DateTimeInterval"/> of the year that includes the current date and time.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTimeInterval GetYearInterval(this DateTime dateTime)
        {
            DateTime startDateTime = new(dateTime.Year, 1, 1);
            DateTime endDateTime = startDateTime.AddYears(1)
                                                .AddTicks(-1);
            return new(startDateTime, endDateTime);
        }
    }
}
