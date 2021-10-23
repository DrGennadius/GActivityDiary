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
        /// Generate simple a sample database and return <see cref="DbContext"/>.
        /// </summary>
        /// <param name="dbFilePath">Database file path.</param>
        /// <param name="activityCount">Number of activities.</param>
        /// <param name="cleanStep">Cleaning step.</param>
        /// <returns><see cref="DbContext"/></returns>
        public static DbContext SimpleGenerate(string dbFilePath = null, int activityCount = 1000000, int cleanStep = 100000)
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

        /// <summary>
        /// Generate a sample database and return <see cref="DbContext"/>.
        /// </summary>
        /// <param name="beginDateTime"></param>
        /// <param name="endDateTime"></param>
        /// <param name="dbFilePath"></param>
        /// <param name="activitiesPerDay"></param>
        /// <param name="cleanStep"></param>
        /// <returns></returns>
        public static DbContext Generate(DateTime beginDateTime, DateTime endDateTime, string dbFilePath = null, int activitiesPerDay = 24, int cleanStep = 100000)
        {
            DbContext db = string.IsNullOrEmpty(dbFilePath) ? new() : new(dbFilePath);

            DateTime curentDateTime = beginDateTime;

            TimeSpan timeSpan = endDateTime - beginDateTime;
            double days = timeSpan.TotalDays;

            TimeSpan activityTimeSpan = TimeSpan.FromDays(1) / activitiesPerDay;

            db.BeginTransaction();

            Tag defaultTag = new("default");
            defaultTag.Id = db.Tags.Save(defaultTag);

            int n = 0;
            for (int d = 0; d < days; d++)
            {
                TimeSpan currentActivityStartAt = TimeSpan.Zero;
                TimeSpan currentActivityAndAt = currentActivityStartAt + activityTimeSpan;
                for (int a = 0; a < activitiesPerDay; a++)
                {
                    Activity activity = new()
                    {
                        Name = $"Test Activity {++n}",
                        CreatedAt = curentDateTime.Date + (currentActivityStartAt + currentActivityAndAt) / 2,
                        StartAt = curentDateTime.Date + currentActivityStartAt,
                        EndAt = curentDateTime.Date + currentActivityAndAt
                    };
                    currentActivityStartAt += activityTimeSpan;
                    currentActivityAndAt += activityTimeSpan;
                    activity.Tags.Add(defaultTag);
                    db.Activities.Save(activity);
                    if (n % cleanStep == 0)
                    {
                        db.Commit();
                        db.ResetSession();
                        db.BeginTransaction();
                    }
                }
                curentDateTime = curentDateTime.AddDays(1);
            }

            db.Commit();

            return db;
        }
    }
}
