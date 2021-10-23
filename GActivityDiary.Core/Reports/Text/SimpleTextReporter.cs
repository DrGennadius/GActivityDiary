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

        /// <summary>
        /// Grouping type.
        /// </summary>
        public ReportGroupingType GroupingType { get; set; } = ReportGroupingType.Nothing;

        public string GetReport(DateTime dateTime)
        {
            return GetReport(new DateTimeInterval(dateTime));
        }

        public string GetReport(DateTimeInterval dateTimeInterval)
        {
            return GetReport(dateTimeInterval.Start, dateTimeInterval.End);
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
                if (GroupingType == ReportGroupingType.Nothing)
                {
                    stringBuilder.Append(GetGroupReport(activities, "Multiple Activity Report"));
                }
                else
                {
                    stringBuilder.AppendLine("Multiple Activity Report");
                    stringBuilder.AppendLine();
                    if (GroupingType == ReportGroupingType.Day)
                    {
                        var groups = activities.Where(x => x.StartAt.HasValue && x.EndAt.HasValue)
                            .GroupBy(x => x.StartAt.Value.Date)
                            .ToArray();

                        foreach (var group in groups)
                        {
                            double groupHours = group.Sum(x => (x.EndAt.Value - x.StartAt.Value).TotalHours);
                            var (hours, minutes) = TimeConverter.GetHoursAndMinutesFromHours(groupHours);
                            string groupTimePart = ((int)hours).ToString("00") + ':' + ((int)minutes).ToString("00");
                            string groupName = $"{group.Key.ToShortDateString()} - {groupTimePart}";
                            stringBuilder.AppendLine(GetGroupReport(group, groupName));
                        }
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
                DateTime startAt = activity.StartAt.Value;
                DateTime endAt = activity.EndAt.Value;
                timePart = GetDatePart(startAt, endAt);
                TimeSpan timeSpan = activity.EndAt.Value - activity.StartAt.Value;
                double totalHours = timeSpan.TotalHours;
                var (hours, minutes) = TimeConverter.GetHoursAndMinutesFromHours(totalHours);
                if (hours >= 1)
                {
                    timePart += $"{hours} {LanguageProfile.Hour.GetValue((int)hours)} ";
                }
                if (minutes > 0)
                {
                    timePart += $"{minutes} {LanguageProfile.Minute.GetValue((int)minutes)} ";
                }
                timePart += "- ";
            }
            string textReport = timePart + activity.Name;
            if (!string.IsNullOrWhiteSpace(activity.Description))
            {
                textReport += ' ' + activity.Description;
            }
            return textReport;
        }

        /// <summary>
        /// Get text of date part.
        /// </summary>
        /// <param name="startAt"></param>
        /// <param name="endAt"></param>
        /// <returns></returns>
        private string GetDatePart(DateTime startAt, DateTime endAt)
        {
            string result = startAt.Date != endAt.Date
                ? $"[{startAt} - {endAt}]"
                : GroupingType switch
                {
                    ReportGroupingType.Day => $"{startAt.ToShortTimeString()}",
                    _ => $"[{startAt.ToShortDateString()} {startAt.ToShortTimeString()}]",
                };
            return result + " - ";
        }

        /// <summary>
        /// Get report for activities of a group.
        /// </summary>
        /// <param name="activities">Activities.</param>
        /// <param name="groupName">Group name.</param>
        /// <returns></returns>
        private string GetGroupReport(IEnumerable<Activity> activities, string groupName)
        {
            StringBuilder stringBuilder = new();

            if (activities.Any())
            {
                stringBuilder.AppendLine(groupName);
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
    }
}
