using GActivityDiary.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GActivityDiary.Core.Helpers
{
    /// <summary>
    /// Rounding time helper.
    /// </summary>
    public class TimeRounderHelper
    {
        public static void Round(Activity activity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Rounds a <see cref="TimeSpan"/> value to the nearest hour value, 
        /// and rounds midpoint values to the next hour.
        /// Only hours and minutes are taken into calculate.
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public static TimeSpan Round(TimeSpan timeSpan)
        {
            int hours = timeSpan.Hours;
            int minutes = timeSpan.Minutes;
            if (minutes >= 30)
            {
                hours++;
            }
            if (hours > 23)
            {
                hours = 23;
                minutes = 59;
            }
            else
            {
                minutes = 0;
            }
            return new TimeSpan(hours, minutes, 0);
        }

        public static void Ceiling(Activity activity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the smallest hour-rounded value that is greater than or equal to the current value.
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public static TimeSpan Ceiling(TimeSpan timeSpan)
        {
            int hours = timeSpan.Hours;
            int minutes = timeSpan.Minutes;
            if (minutes > 0)
            {
                hours++;
            }
            if (hours > 23)
            {
                hours = 23;
                minutes = 59;
            }
            else
            {
                minutes = 0;
            }
            return new TimeSpan(hours, minutes, 0);
        }

        public static void Floor(Activity activity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the largest hour-rounded value that is less than or equal to the current value.
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public static TimeSpan Floor(TimeSpan timeSpan)
        {
            return new TimeSpan(timeSpan.Hours, 0, 0);
        }
    }
}
