using GActivityDiary.Core.DataBase;
using GActivityDiary.Core.Models;
using System;

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
        /// <param name="dbFilePath"></param>
        /// <param name="activityCount"></param>
        /// <param name="cleanStep"></param>
        /// <returns></returns>
        public static DbContext Generate(string dbFilePath = null, int activityCount = 1000000, int cleanStep = 100000)
        {
            DbContext db = string.IsNullOrEmpty(dbFilePath) ? new() : new(dbFilePath);
            db.BeginTransaction();

            DateTime dateTime = DateTime.Now.AddMonths(-1);

            // TODO: Create a new class to generate sample data.
            for (int i = 1; i <= activityCount; i++)
            {
                db.Activities.Save(new Activity()
                {
                    Name = $"Test Activity {i}",
                    CreatedAt = dateTime
                });
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
