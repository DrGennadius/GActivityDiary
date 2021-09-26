using GActivityDiary.Core.DataBase;
using GActivityDiary.Core.Models;
using System;
using System.Collections.Generic;

namespace GActiveDiary.Tests.Common.DataBaseUtils
{
    /// <summary>
    /// DataBase Generator. Helps generate massive data for debug and tests.
    /// </summary>
    public class DataBaseGenerator
    {
        /// <summary>
        /// Generate sample database and return <see cref="DbContext"/>.
        /// </summary>
        /// <param name="dbFilePath">Database file path.</param>
        /// <param name="activityCount">Number of activities.</param>
        /// <param name="cleanStep">Cleaning step.</param>
        /// <returns><see cref="DbContext"/></returns>
        public static DbContext Generate(string dbFilePath = null, int activityCount = 1000000, int cleanStep = 100000)
        {
            DbContext db = string.IsNullOrEmpty(dbFilePath) ? new() : new(dbFilePath);
            db.BeginTransaction();

            DateTime dateTime = DateTime.Now.AddMonths(-1);

            Tag defaultTag = new("default");
            defaultTag.Id = db.Tags.Save(defaultTag);

            for (int i = 1; i <= activityCount; i++)
            {
                Activity activity = new()
                {
                    Name = $"Test Activity {i}",
                    CreatedAt = dateTime,
                    StartAt = dateTime,
                    EndAt = dateTime.AddHours(1)
                };
                activity.Tags.Add(defaultTag);
                db.Activities.Save(activity);
                dateTime = dateTime.AddMinutes(1);
                if (i % cleanStep == 0)
                {
                    db.Commit();
                    db.ResetSession();
                    db.BeginTransaction();
                }
            }

            db.Commit();

            return db;
        }
    }
}
