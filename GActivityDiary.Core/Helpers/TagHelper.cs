using GActivityDiary.Core.DataBase;
using GActivityDiary.Core.Models;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GActivityDiary.Core.Helpers
{
    /// <summary>
    /// <see cref="Tag"/> helper.
    /// </summary>
    public static class TagHelper
    {
        /// <summary>
        /// Get or create tags by a text sequence of tags.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="tagsString"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<Tag>> GetOrCreateTagsAsync(DbContext dbContext, string tagsString)
        {
            if (dbContext is null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            List<Tag> tags = new();

            if (string.IsNullOrWhiteSpace(tagsString))
            {
                return tags;
            }

            var tagStrings = Regex.Split(tagsString.ToLower(), @"\W+");
            var existsTags = await GetTagsAsync(dbContext, tagStrings);

            var (transaction, isNew) = dbContext.GetCurrentTransactionOrCreateNew();
            foreach (var tagString in tagStrings)
            {
                if (string.IsNullOrEmpty(tagString))
                {
                    continue;
                }
                Tag tag = existsTags.FirstOrDefault(x => x.Name == tagString);
                if (tag == null)
                {
                    tag = new(tagString);
                    tag.Id = dbContext.Tags.Save(tag);
                }
                tags.Add(tag);
            }
            if (isNew)
            {
                transaction.Commit();
            }

            return tags.AsEnumerable();
        }

        /// <summary>
        /// Get tags by a text sequence of tags.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="tagsString"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<Tag>> GetTagsAsync(DbContext dbContext, string tagsString)
        {
            if (dbContext is null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            if (string.IsNullOrWhiteSpace(tagsString))
            {
                return new List<Tag>();
            }

            var tagStrings = Regex.Split(tagsString.ToLower(), @"\W+");
            var existsTags = await dbContext.Tags.FindAsync(x => tagStrings.Contains(x.Name));
            return existsTags.AsEnumerable();
        }

        /// <summary>
        /// Get tags by a text tag array.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<Tag>> GetTagsAsync(DbContext dbContext, string[] tags)
        {
            if (dbContext is null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            if (tags.Length == 0)
            {
                return new List<Tag>();
            }

            var existsTags = await dbContext.Tags.FindAsync(x => tags.Contains(x.Name));
            return existsTags.AsEnumerable();
        }

        /// <summary>
        /// Get tag ids by a text sequence of tags.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="tagsString"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<Guid>> GetTagIdsAsync(DbContext dbContext, string tagsString)
        {
            if (dbContext is null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            if (string.IsNullOrWhiteSpace(tagsString))
            {
                return new List<Guid>();
            }

            var tagStrings = Regex.Split(tagsString.ToLower(), @"\W+");
            var ids = await dbContext.Session.CreateCriteria(typeof(Tag))
                                             .Add(Restrictions.In("Name", tagStrings))
                                             .SetProjection(Projections.Property("Id"))
                                             .ListAsync<Guid>();
            return ids.AsEnumerable();
        }

        /// <summary>
        /// Get tag ids by a text tag array.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<Guid>> GetTagIdsAsync(DbContext dbContext, string[] tags)
        {
            if (dbContext is null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            if (tags.Length == 0)
            {
                return new List<Guid>();
            }

            var ids = await dbContext.Session.CreateCriteria(typeof(Tag))
                                             .Add(Restrictions.In("Name", tags))
                                             .SetProjection(Projections.Property("Id"))
                                             .ListAsync<Guid>();
            return ids.AsEnumerable();
        }
    }
}
