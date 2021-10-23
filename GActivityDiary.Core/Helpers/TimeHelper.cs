using GActivityDiary.Core.Common;
using GActivityDiary.Core.Extensions;
using System;

namespace GActivityDiary.Core.Helpers
{
    /// <summary>
    /// Time helper.
    /// </summary>
    public static class TimeHelper
    {
        /// <summary>
        /// Get the <see cref="DateTimeInterval"/> for today.
        /// </summary>
        /// <returns></returns>
        public static DateTimeInterval GetDayInterval()
        {
            return new(DateTime.Now);
        }

        /// <summary>
        /// Get the <see cref="DateTimeInterval"/> for this week.
        /// </summary>
        /// <returns></returns>
        public static DateTimeInterval GetWeekInterval()
        {
            return DateTime.Now.GetWeekInterval();
        }

        /// <summary>
        /// Get the <see cref="DateTimeInterval"/> for this month.
        /// </summary>
        /// <returns></returns>
        public static DateTimeInterval GetMonthInterval()
        {
            return DateTime.Now.GetMonthInterval();
        }

        /// <summary>
        /// Get the <see cref="DateTimeInterval"/> for this quarter.
        /// </summary>
        /// <returns></returns>
        public static DateTimeInterval GetQuarterInterval()
        {
            return DateTime.Now.GetQuarterInterval();
        }

        /// <summary>
        /// Get the <see cref="DateTimeInterval"/> for this year.
        /// </summary>
        /// <returns></returns>
        public static DateTimeInterval GetYearInterval()
        {
            return DateTime.Now.GetYearInterval();
        }
    }
}
