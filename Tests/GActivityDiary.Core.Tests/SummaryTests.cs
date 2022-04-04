using GActiveDiary.Tests.Common.DataBaseUtils;
using GActivityDiary.Core.Common;
using GActivityDiary.Core.DataBase;
using GActivityDiary.Core.Helpers;
using GActivityDiary.Core.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GActivityDiary.Core.Tests
{
    public class SummaryTests
    {
        private const string _testDBFilePath = "test.db";
        private DbContext _db;

        [SetUp]
        public void Setup()
        {
            if (_db != null)
            {
                return;
            }
            if (File.Exists(_testDBFilePath))
            {
                File.Delete(_testDBFilePath);
            }
            DateTime now = DateTime.Now;
            _db = DataBaseGenerator.Generate(beginDateTime: now.AddMonths(-1),
                                             endDateTime: now.AddDays(-7),
                                             dbFilePath: _testDBFilePath,
                                             activitiesPerDay: 4);
        }

        /// <summary>
        /// [TotalHours] Single activity. 1 Simple part.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task SingleTotalHoursTest1Async()
        {
            DateTime firstDay = DateTime.Today.AddDays(-6);

            Activity activity = new("Test1")
            {
                StartAt = firstDay.AddHours(7),
                EndAt = firstDay.AddHours(10)
            };
            await _db.Activities.SaveAsync(activity);

            double hours = ActivityHelper.GetTotalHours(activity);
            Assert.AreEqual(3, hours);

            hours = ActivityHelper.GetTotalHours(activity, new DateTimeInterval(firstDay.AddHours(8), firstDay.AddHours(9)));
            Assert.AreEqual(1, hours);

            hours = ActivityHelper.GetTotalHours(activity, firstDay);
            Assert.AreEqual(3, hours);

            Assert.Pass();
        }

        /// <summary>
        /// [TotalHours] Single activity. 2 Multiple days.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task SingleTotalHoursTest2Async()
        {
            DateTime firstDay = DateTime.Today.AddDays(-6);

            Activity activity = new("Test1")
            {
                StartAt = firstDay,
                EndAt = firstDay.AddDays(3)
            };
            await _db.Activities.SaveAsync(activity);

            double hours = ActivityHelper.GetTotalHours(activity);
            Assert.AreEqual(24 * 3, hours);

            hours = ActivityHelper.GetTotalHours(activity, new DateTimeInterval(firstDay.AddHours(8), firstDay.AddHours(9)));
            Assert.AreEqual(1, hours);

            hours = ActivityHelper.GetTotalHours(activity, firstDay);
            Assert.AreEqual(24, hours);

            Assert.Pass();
        }

        /// <summary>
        /// [TotalHours] Multiple activities.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task MultipleTotalHoursTestAsync()
        {
            DateTime firstDay = DateTime.Today.AddDays(-6);
            List<Activity> activities = GenerateActivities(firstDay);

            foreach (var activity in activities)
            {
                await _db.Activities.SaveAsync(activity);
            }

            // Total Hours for all activities.
            double hours = ActivityHelper.GetTotalHours(activities);
            Assert.AreEqual(activities.Sum(x => (x.EndAt - x.StartAt).Value.TotalHours), hours);

            // Total hours from first day for 1 year.
            hours = ActivityHelper.GetTotalHours(activities, new DateTimeInterval(firstDay, firstDay.AddYears(1)));
            Assert.AreEqual(activities.Sum(x => (x.EndAt - x.StartAt).Value.TotalHours), hours);

            // Total hours from first day for 7 days.
            hours = ActivityHelper.GetTotalHours(activities, new DateTimeInterval(firstDay, firstDay.AddDays(7)));
            Assert.AreEqual(activities.Sum(x => (x.EndAt - x.StartAt).Value.TotalHours) - 8, hours);

            // Total hours for the first day.
            hours = ActivityHelper.GetTotalHours(activities, firstDay);
            Assert.AreEqual(8, hours);

            // Check total hours for the empty day (3).
            hours = ActivityHelper.GetTotalHours(activities, firstDay.AddDays(3));
            Assert.AreEqual(0, hours);

            // Check total hours for the 4th day.
            hours = ActivityHelper.GetTotalHours(activities, firstDay.AddDays(4));
            Assert.AreEqual(24 - 8, hours);

            // Check total hours for the 5th day.
            hours = ActivityHelper.GetTotalHours(activities, firstDay.AddDays(5));
            Assert.AreEqual(24, hours);

            // Check total hours for the last 2 activity for 10 days from first day.
            hours = ActivityHelper.GetTotalHours(activities, new DateTimeInterval(activities[^2].StartAt.Value, firstDay.AddDays(10)));
            Assert.AreEqual(hours, (activities[^1].EndAt.Value - activities[^2].StartAt.Value).TotalHours);

            Assert.Pass();
        }

        private static List<Activity> GenerateActivities(DateTime firstDay)
        {
            return new()
            {
                new Activity("Test1")
                {
                    StartAt = firstDay.AddHours(8),
                    EndAt = firstDay.AddHours(12)
                },
                new Activity("Test2")
                {
                    StartAt = firstDay.AddHours(13),
                    EndAt = firstDay.AddHours(17)
                },
                new Activity("Test3")
                {
                    StartAt = firstDay.AddDays(1).AddHours(10),
                    EndAt = firstDay.AddDays(1).AddHours(11)
                },
                new Activity("Test4")
                {
                    StartAt = firstDay.AddDays(2).AddHours(8),
                    EndAt = firstDay.AddDays(2).AddHours(12)
                },
                new Activity("Test5")
                {
                    StartAt = firstDay.AddDays(4).AddHours(8),
                    EndAt = firstDay.AddDays(5)
                },
                new Activity("Test6")
                {
                    StartAt = firstDay.AddDays(5),
                    EndAt = firstDay.AddDays(6)
                },
                new Activity("Test7")
                {
                    StartAt = firstDay.AddDays(6),
                    EndAt = firstDay.AddDays(7).AddHours(8)
                }
            };
        }

        [Test]
        public async Task CostTestAsync()
        {
            Assert.Pass();
        }
    }
}
