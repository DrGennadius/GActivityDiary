using GActivityDiary.Core.Common;
using GActivityDiary.Core.DataBase;
using GActivityDiary.Core.Models;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GActivityDiary.Core.Helpers
{
    /// <summary>
    /// Activity helper.
    /// </summary>
    public static class ActivityHelper
    {
        /// <summary>
        /// Get activities by a text sequence of tags.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="tagsString"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<Activity>> GetByTagsAsync(DbContext dbContext, string tagsString)
        {
            if (dbContext is null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            if (string.IsNullOrEmpty(tagsString))
            {
                return Enumerable.Empty<Activity>();
            }

            var tagIds = await TagHelper.GetTagIdsAsync(dbContext, tagsString);
            if (!tagIds.Any())
            {
                return Enumerable.Empty<Activity>();
            }
            var activities = await dbContext.Session.CreateCriteria<Activity>()
                                                    .CreateCriteria("Tags")
                                                    .Add(Restrictions.In("Id", tagIds.ToArray()))
                                                    .ListAsync<Activity>();
            return activities;
        }

        /// <summary>
        /// Get activities by a text sequence of tags.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="tagsString"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<Activity>> GetByTagsAsync(DbContext dbContext, string tagsString, int pageIndex, int pageSize)
        {
            if (dbContext is null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            if (string.IsNullOrWhiteSpace(tagsString))
            {
                return await dbContext.Session.CreateCriteria<Activity>()
                                              .SetFirstResult(pageIndex * pageSize)
                                              .SetMaxResults(pageSize)
                                              .ListAsync<Activity>();
            }

            var tagIds = await TagHelper.GetTagIdsAsync(dbContext, tagsString);
            if (!tagIds.Any())
            {
                return Enumerable.Empty<Activity>();
            }
            return await dbContext.Session.CreateCriteria<Activity>()
                                          .CreateCriteria("Tags")
                                          .Add(Restrictions.In("Id", tagIds.ToArray()))
                                          .SetFirstResult(pageIndex * pageSize)
                                          .SetMaxResults(pageSize)
                                          .ListAsync<Activity>();
        }

        /// <summary>
        /// Get activities by a text sequence of tags.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="tagsString"></param>
        /// <returns></returns>
        public static async Task<int> GetCountByTagsAsync(DbContext dbContext, string tagsString)
        {
            if (dbContext is null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            if (string.IsNullOrWhiteSpace(tagsString))
            {
                return dbContext.Activities.Query().Count();
            }

            var tagIds = await TagHelper.GetTagIdsAsync(dbContext, tagsString);
            if (!tagIds.Any())
            {
                return 0;
            }
            return await dbContext.Session.CreateCriteria<Activity>()
                                          .CreateCriteria("Tags")
                                          .Add(Restrictions.In("Id", tagIds.ToArray()))
                                          .SetProjection(Projections.Count(Projections.Id()))
                                          .UniqueResultAsync<int>();
        }

        public static IEnumerable<Activity> GetWithinPeriod(DbContext dbContext, DateTimeInterval interval)
        {
            // ∃xn && ∃yn && !(x1 < y1 && x2 < y2) && !(x1 > y1 && x2 > y2)
            return dbContext.Activities.Find(x => !(x.StartAt < interval.Start && x.EndAt < interval.End)
                                               && !(x.StartAt > interval.Start && x.EndAt > interval.End));
        }

        public static async Task<IEnumerable<Activity>> GetWithinPeriodAsync(DbContext dbContext, DateTimeInterval interval)
        {
            // ∃xn && ∃yn && !(x1 < y1 && x2 < y2) && !(x1 > y1 && x2 > y2)
            return await dbContext.Activities.FindAsync(x => !(x.StartAt < interval.Start && x.EndAt < interval.End)
                                                          && !(x.StartAt > interval.Start && x.EndAt > interval.End));
        }

        public static IEnumerable<Activity> GetWithinPeriod(IEnumerable<Activity> activities, DateTimeInterval interval)
        {
            // ∃xn && ∃yn && !(x1 < y1 && x2 < y2) && !(x1 > y1 && x2 > y2)
            return activities.Where(x => !(x.StartAt < interval.Start && x.EndAt < interval.End)
                                      && !(x.StartAt > interval.Start && x.EndAt > interval.End));
        }

        public static IEnumerable<Activity> GetWithinPeriod(IEnumerable<Activity> activities, DateTime day)
        {
            return GetWithinPeriod(activities, new DateTimeInterval(day));
        }

        public static double GetTotalHours(Activity activity)
        {
            if (activity is null)
            {
                throw new ArgumentNullException(nameof(activity));
            }

            var duration = activity.GetDuration();
            return duration.HasValue ? duration.Value.TotalHours : 0;
        }

        public static double GetTotalHours(Activity activity, DateTimeInterval interval)
        {
            if (activity is null)
            {
                throw new ArgumentNullException(nameof(activity));
            }
            if (!IsWithin(activity, interval))
            {
                return 0;
            }

            TimeSpan duration = GetDuration(activity, interval);
            return duration.TotalHours;
        }

        public static double GetTotalHours(Activity activity, DateTime day)
        {
            return GetTotalHours(activity, new DateTimeInterval(day));
        }

        /// <summary>
        /// Get total hours for the activities.
        /// </summary>
        /// <param name="activities"></param>
        /// <returns></returns>
        public static double GetTotalHours(IEnumerable<Activity> activities)
        {
            double hours = 0;

            foreach (var item in activities)
            {
                var duration = item.GetDuration();
                if (duration.HasValue)
                {
                    hours += duration.Value.TotalHours;
                }
            }

            return hours;
        }

        /// <summary>
        /// Get total hours for the activities by the date interval.
        /// </summary>
        /// <param name="activities"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        public static double GetTotalHours(IEnumerable<Activity> activities, DateTimeInterval interval)
        {
            double hours = 0;

            foreach (var item in activities)
            {
                if (!item.StartAt.HasValue && !item.EndAt.HasValue)
                {
                    continue;
                }
                if (!IsWithin(item, interval))
                {
                    continue;
                }
                TimeSpan duration = GetDuration(item, interval);
                hours += duration.TotalHours;
            }

            return hours;
        }

        public static bool IsWithin(Activity activity, DateTimeInterval interval)
        {
            if (activity is null)
            {
                throw new ArgumentNullException(nameof(activity));
            }
            var activityInterval = activity.GetInterval();
            if (!activityInterval.HasValue)
            {
                return false;
            }
            return interval.IsWithin(activityInterval!);
        }

        private static TimeSpan GetDuration(Activity activity, DateTimeInterval interval)
        {
            DateTime startDate = GetStartDateTime(activity.StartAt, interval.Start);
            DateTime endDate = GetEndDateTime(activity.EndAt, interval.End);
            return endDate - startDate;
        }

        /// <summary>
        /// Get total hours for the activities by the day.
        /// </summary>
        /// <param name="activities"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public static double GetTotalHours(IEnumerable<Activity> activities, DateTime day)
        {
            return GetTotalHours(activities, new DateTimeInterval(day));
        }
        
        public static decimal GetTotalCost(IEnumerable<Activity> activities, DateTimeInterval interval)
        {
            decimal cost = 0;

            foreach (var item in activities)
            {
                if (item.ActivityType == null || item.ActivityType.Cost == 0)
                {
                    continue;
                }
                TimeSpan duration = GetDuration(item, interval);
                cost += item.ActivityType.Cost * (decimal)duration.TotalHours;
            }

            return cost;
        }

        public static decimal GetTotalCost(IEnumerable<Activity> activities, DateTime day)
        {
            return GetTotalCost(activities, new DateTimeInterval(day));
        }

        private static DateTime GetStartDateTime(DateTime? dateTime, DateTime minDateTime)
        {
            return dateTime.HasValue ? minDateTime > dateTime.Value ? minDateTime : dateTime.Value : minDateTime;
        }

        private static DateTime GetEndDateTime(DateTime? dateTime, DateTime maxDateTime)
        {
            return dateTime.HasValue ? maxDateTime < dateTime.Value ? maxDateTime : dateTime.Value : maxDateTime;
        }
    }
}
