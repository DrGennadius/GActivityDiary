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
    }
}
