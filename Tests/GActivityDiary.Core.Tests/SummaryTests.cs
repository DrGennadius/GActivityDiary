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
using System.Text;
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

        [Test]
        public async Task SingleTotalHoursTestAsync()
        {
            DateTime firstDay = DateTime.Today.AddDays(-6);

            // 1. Simple part

            Activity activity = new("Test1")
            {
                StartAt = firstDay.AddHours(7),
                EndAt = firstDay.AddHours(10)
            };
            await _db.Activities.SaveAsync(activity);

            double hours = ActivityHelper.GetTotalHours(activity);
            Assert.AreEqual(hours, 3);

            hours = ActivityHelper.GetTotalHours(activity, new DateTimeInterval(firstDay.AddHours(8), firstDay.AddHours(9)));
            Assert.AreEqual(hours, 1);

            hours = ActivityHelper.GetTotalHours(activity, firstDay);
            Assert.AreEqual(hours, 3);

            // 2. Multiple days

            activity = new("Test2")
            {
                StartAt = firstDay,
                EndAt = firstDay.AddDays(3)
            };
            await _db.Activities.SaveAsync(activity);

            hours = ActivityHelper.GetTotalHours(activity);
            Assert.AreEqual(hours, 24 * 3);

            hours = ActivityHelper.GetTotalHours(activity, new DateTimeInterval(firstDay.AddHours(8), firstDay.AddHours(9)));
            Assert.AreEqual(hours, 1);

            hours = ActivityHelper.GetTotalHours(activity, firstDay);
            Assert.AreEqual(hours, 24);

            Assert.Pass();
        }

        [Test]
        public async Task MultipleTotalHoursTestAsync()
        {
            DateTime firstDay = DateTime.Today.AddDays(-6);

            // 1. Simple part

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

            double hours = ActivityHelper.GetTotalHours(activities);
            Assert.AreEqual(hours, activities.Sum(x => (x.EndAt - x.StartAt).Value.TotalHours));

            hours = ActivityHelper.GetTotalHours(activities, firstDay.AddDays(3));
            Assert.AreEqual(hours, 0);

            Assert.Pass();
        }

        [Test]
        public async Task CostTestAsync()
        {
            Assert.Pass();
        }
    }
}
