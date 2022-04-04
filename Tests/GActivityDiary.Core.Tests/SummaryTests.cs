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
            List<Activity> activities = await GenerateActivitiesAsync(firstDay);

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
            Assert.AreEqual((activities[^1].EndAt.Value - activities[^2].StartAt.Value).TotalHours, hours);

            Assert.Pass();
        }

        [Test]
        public async Task CostTestAsync()
        {
            DateTime firstDay = DateTime.Today.AddDays(-6);
            List<Activity> activities = await GenerateActivitiesAsync(firstDay);
            await SetTypeForActivitiesAsync(activities);

            // Total cost for all activities.
            decimal cost = ActivityHelper.GetTotalCost(activities);
            int i = 0;
            decimal expectedCost = (decimal)activities.Sum(x => (x.EndAt - x.StartAt).Value.TotalHours * 100 * ++i);
            Assert.AreEqual(expectedCost, cost);

            // Total cost for the first day.
            cost = ActivityHelper.GetTotalCost(activities, firstDay);
            Assert.AreEqual(4 * 100 + 4 * 200, cost);

            // Check total cost for the last 2 activity for 10 days from first day.
            cost = ActivityHelper.GetTotalCost(activities, new DateTimeInterval(activities[^2].StartAt.Value, firstDay.AddDays(10)));
            expectedCost = activities[^1].ActivityType.Cost
                           * (decimal)activities[^1].GetDuration().Value.TotalHours
                           + activities[^2].ActivityType.Cost
                           * (decimal)activities[^2].GetDuration().Value.TotalHours;
            Assert.AreEqual(expectedCost, cost);

            Assert.Pass();
        }

        private async Task<List<Activity>> GenerateActivitiesAsync(DateTime firstDay)
        {
            List<Activity> activities = new()
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

            foreach (var activity in activities)
            {
                await _db.Activities.SaveAsync(activity);
            }

            return activities;
        }

        private async Task SetTypeForActivitiesAsync(List<Activity> activities)
        {
            int i = 0;
            foreach (var acitivity in activities)
            {
                i++;
                acitivity.ActivityType = new($"Type{i}", i * 100);
                await _db.Activities.SaveAsync(acitivity);
            }
        }
    }
}
