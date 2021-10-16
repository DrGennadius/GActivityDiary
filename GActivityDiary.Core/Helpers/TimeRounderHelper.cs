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
        /// <summary>
        /// Rounds a start and end datetimes to the nearest hour values, 
        /// and rounds midpoint values to the next hour.
        /// Only hours and minutes are taken into calculate.
        /// </summary>
        /// <param name="activity"></param>
        public static void Round(Activity activity)
        {
            if (activity == null)
            {
                throw new ArgumentNullException();
            }
            if (!activity.StartAt.HasValue && !activity.EndAt.HasValue)
            {
                return;
            }
            if (activity.StartAt.HasValue
                && activity.EndAt.HasValue
                && activity.StartAt.Value >= activity.EndAt.Value)
            {
                activity.EndAt = activity.StartAt?.AddMinutes(30);
            }
            if (activity.StartAt.HasValue)
            {
                activity.StartAt = Round(activity.StartAt.Value);
            }
            if (activity.EndAt.HasValue)
            {
                activity.EndAt = Round(activity.EndAt.Value);
            }
        }

        /// <summary>
        /// Rounds a <see cref="DateTime"/> value to the nearest hour value, 
        /// and rounds midpoint values to the next hour.
        /// Only hours and minutes are taken into calculate.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime Round(DateTime dateTime)
        {
            TimeSpan roundedTimeSpan = Round(dateTime.TimeOfDay);
            return new DateTime(
                dateTime.Year, 
                dateTime.Month, 
                dateTime.Day, 
                roundedTimeSpan.Hours, 
                roundedTimeSpan.Minutes, 
                0);
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

        /// <summary>
        /// Returns the smallest hour-rounded value that is greater than or equal to the current values.
        /// </summary>
        /// <param name="activity"></param>
        public static void Ceiling(Activity activity)
        {
            if (activity == null)
            {
                throw new ArgumentNullException();
            }
            if (!activity.StartAt.HasValue && !activity.EndAt.HasValue)
            {
                return;
            }
            if (activity.StartAt.HasValue
                && activity.EndAt.HasValue
                && activity.StartAt >= activity.EndAt)
            {
                activity.EndAt = activity.StartAt?.AddMinutes(30);
            }
            if (activity.StartAt.HasValue)
            {
                activity.StartAt = Ceiling(activity.StartAt.Value);
            }
            if (activity.EndAt.HasValue)
            {
                activity.EndAt = Ceiling(activity.EndAt.Value);
            }
        }

        /// <summary>
        /// Returns the smallest hour-rounded value that is greater than or equal to the current value.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime Ceiling(DateTime dateTime)
        {
            TimeSpan roundedTimeSpan = Ceiling(dateTime.TimeOfDay);
            return new DateTime(
                dateTime.Year,
                dateTime.Month,
                dateTime.Day,
                roundedTimeSpan.Hours,
                roundedTimeSpan.Minutes,
                0);
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

        /// <summary>
        /// Returns the largest hour-rounded start and end datetimes 
        /// that is less than or equal to the current values.
        /// </summary>
        /// <param name="activity"></param>
        public static void Floor(Activity activity)
        {
            if (activity == null)
            {
                throw new ArgumentNullException();
            }
            if (!activity.StartAt.HasValue && !activity.EndAt.HasValue)
            {
                return;
            }
            if (activity.StartAt.HasValue
                && activity.EndAt.HasValue
                && activity.StartAt >= activity.EndAt)
            {
                activity.EndAt = activity.StartAt?.AddMinutes(30);
            }
            if (activity.StartAt.HasValue)
            {
                activity.StartAt = Floor(activity.StartAt.Value);
            }
            if (activity.EndAt.HasValue)
            {
                activity.EndAt = Floor(activity.EndAt.Value);
            }
        }

        /// <summary>
        /// Returns the largest hour-rounded value that is less than or equal to the current value.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime Floor(DateTime dateTime)
        {
            TimeSpan roundedTimeSpan = Floor(dateTime.TimeOfDay);
            return new DateTime(
                dateTime.Year,
                dateTime.Month,
                dateTime.Day,
                roundedTimeSpan.Hours,
                roundedTimeSpan.Minutes,
                0);
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
