﻿using System;

namespace GActivityDiary.Core.Converters.Time
{
    /// <summary>
    /// Simple converter for hours and minutes.
    /// </summary>
    public class TimeConverter
    {
        /// <summary>
        /// Get total minutes.
        /// </summary>
        /// <param name="hours"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static double GetMinutes(double hours, double minutes = 0)
        {
            return hours * 60 + minutes;
        }

        /// <summary>
        /// Get hours from minutes.
        /// </summary>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static double GetHours(double minutes)
        {
            return minutes / 60;
        }

        /// <summary>
        /// Get total hours.
        /// </summary>
        /// <param name="hours"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static double GetHours(double hours, double minutes)
        {
            return minutes / 60 + hours;
        }

        /// <summary>
        /// Get hours and minutes from total minutes.
        /// </summary>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static (double, double) GetHoursAndMinutes(double minutes)
        {
            double truncatedHours = Math.Truncate(minutes / 60);
            return (truncatedHours, minutes - truncatedHours * 60);
        }

        /// <summary>
        /// Get hours and minutes from total hours.
        /// </summary>
        /// <param name="hours"></param>
        /// <returns></returns>
        public static (double, double) GetHoursAndMinutesFromHours(double hours)
        {
            double truncatedHours = Math.Truncate(hours);
            return (truncatedHours, (hours - truncatedHours) * 60);
        }
    }
}
