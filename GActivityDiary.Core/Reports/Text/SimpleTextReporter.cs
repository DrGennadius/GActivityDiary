using GActivityDiary.Core.Common;
using GActivityDiary.Core.Converters.Time;
using GActivityDiary.Core.DataBase;
using GActivityDiary.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GActivityDiary.Core.Reports.Text
{
    /// <summary>
    /// Simple text reporter. Creates a reports in text format.
    /// </summary>
    public class SimpleTextReporter : ITextReporter
    {
        public SimpleTextReporter(DbContext dbContext, LanguageProfile languageProfile)
        {
            DbContext = dbContext;
            LanguageProfile = languageProfile;
        }

        /// <summary>
        /// Database context.
        /// </summary>
        public DbContext DbContext { get; private set; }

        /// <summary>
        /// Language profile.
        /// </summary>
        public LanguageProfile LanguageProfile { get; private set; }

        public string GetReport(DateTime dateTime)
        {
            return GetReport(
                new DateTime(dateTime.Year, dateTime.Month, dateTime.Day),
                new DateTime(dateTime.Year, dateTime.Month, dateTime.Day).AddDays(1)
                                                                         .AddMilliseconds(-1));
        }

        public string GetReport(DateTime beginDateTime, TimeSpan timeSpan)
        {
            return GetReport(
                beginDateTime,
                beginDateTime.Add(timeSpan));
        }

        public string GetReport(DateTime beginDateTime, DateTime endDateTime)
        {
            IQueryable<Activity> activities = DbContext.Activities.Query();
            return GetReport(
                activities.Where(x => x.StartAt >= beginDateTime && x.EndAt <= endDateTime)
                          .AsEnumerable());
        }

        public string GetReport(IEnumerable<Activity> activities)
        {
            StringBuilder stringBuilder = new();

            if (activities.Any())
            {
                stringBuilder.AppendLine("Multiple Activity Report");
                stringBuilder.AppendLine();
                foreach (var activity in activities)
                {
                    string avtivityReport = GetReport(activity);
                    if (!string.IsNullOrEmpty(avtivityReport))
                    {
                        stringBuilder.AppendLine(avtivityReport);
                    }
                }
            }

            return stringBuilder.ToString();
        }

        public string GetReport(Activity activity)
        {
            if (activity == null)
            {
                return "";
            }
            string timePart = "";
            if (activity.StartAt.HasValue && activity.EndAt.HasValue)
            {
                timePart = $"{activity.StartAt}-{activity.EndAt}";
                TimeSpan timeSpan = activity.EndAt.Value - activity.StartAt.Value;
                double totalHours = timeSpan.TotalHours;
                var (hours, minutes) = TimeConverter.GetHoursAndMinutesFromHours(totalHours);
                if (hours >= 1)
                {
                    timePart += $" - {hours} {LanguageProfile.Hour.GetValue((int)hours)}";
                }
                if (minutes > 0)
                {
                    timePart += $" {minutes} {LanguageProfile.Minute.GetValue((int)minutes)}";
                }
                timePart += " - ";
            }
            string textReport = timePart + activity.Name;
            if (!string.IsNullOrWhiteSpace(activity.Description))
            {
                textReport += ' ' + activity.Description;
            }
            return textReport;
        }
    }
}
