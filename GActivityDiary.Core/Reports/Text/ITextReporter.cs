using GActivityDiary.Core.Common;
using GActivityDiary.Core.Models;
using System;
using System.Collections.Generic;

namespace GActivityDiary.Core.Reports.Text
{
    /// <summary>
    /// Text reporter. Creates a reports in text format.
    /// </summary>
    interface ITextReporter
    {
        /// <summary>
        /// Create a specific day report.
        /// </summary>
        /// <param name="dateTime">Day.</param>
        /// <returns></returns>
        string GetReport(DateTime dateTime);

        /// <summary>
        /// Create a report by <see cref="DateTimeInterval"/>.
        /// </summary>
        /// <param name="dateTimeInterval"></param>
        /// <returns></returns>
        string GetReport(DateTimeInterval dateTimeInterval);

        /// <summary>
        /// Create a report starting from a specified date for a specific period of time.
        /// </summary>
        /// <param name="beginDateTime">Begin date and time.</param>
        /// <param name="timeSpan">Time span.</param>
        /// <returns></returns>
        string GetReport(DateTime beginDateTime, TimeSpan timeSpan);

        /// <summary>
        /// Create a report from a specific date to a specific date.
        /// </summary>
        /// <param name="beginDateTime">Begin date.</param>
        /// <param name="endDateTime">End date.</param>
        /// <returns></returns>
        string GetReport(DateTime beginDateTime, DateTime endDateTime);

        /// <summary>
        /// Create a report for multiple activities.
        /// </summary>
        /// <param name="activities"></param>
        /// <returns></returns>
        string GetReport(IEnumerable<Activity> activities);

        /// <summary>
        /// Create a report for multiple activities by the date time interval (<see cref="DateTimeInterval"/>).
        /// </summary>
        /// <param name="activities"></param>
        /// <param name="dateTimeInterval"></param>
        /// <returns></returns>
        string GetReport(IEnumerable<Activity> activities, DateTimeInterval dateTimeInterval);

        /// <summary>
        /// Create a report for multiple activities by the date time interval (from begin to end).
        /// </summary>
        /// <param name="activities"></param>
        /// <param name="beginDateTime"></param>
        /// <param name="endDateTime"></param>
        /// <returns></returns>
        string GetReport(IEnumerable<Activity> activities, DateTime beginDateTime, DateTime endDateTime);

        /// <summary>
        /// Create a report for an activity.
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        string GetReport(Activity activity);
    }
}
