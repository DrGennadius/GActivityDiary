using GActivityDiary.Core.Helpers;
using GActivityDiary.Core.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GActivityDiary.Core.Tests.DateTimeOperations
{
    public class RoundingTimeTests
    {
        [Test]
        public void TimeSpanRoundingTest()
        {
            TimeSpan timeSpan1 = new(1, 0, 0);
            TimeSpan timeSpan2 = new(4, 15, 0);
            TimeSpan timeSpan3 = new(8, 30, 0);
            TimeSpan timeSpan4 = new(23, 30, 0);

            // Round
            Assert.AreEqual(TimeRounderHelper.Round(timeSpan1), new TimeSpan(1, 0, 0));
            Assert.AreEqual(TimeRounderHelper.Round(timeSpan2), new TimeSpan(4, 0, 0));
            Assert.AreEqual(TimeRounderHelper.Round(timeSpan3), new TimeSpan(9, 0, 0));
            Assert.AreEqual(TimeRounderHelper.Round(timeSpan4), new TimeSpan(23, 59, 0));

            // Floor
            Assert.AreEqual(TimeRounderHelper.Floor(timeSpan1), new TimeSpan(1, 0, 0));
            Assert.AreEqual(TimeRounderHelper.Floor(timeSpan2), new TimeSpan(4, 0, 0));
            Assert.AreEqual(TimeRounderHelper.Floor(timeSpan3), new TimeSpan(8, 0, 0));
            Assert.AreEqual(TimeRounderHelper.Floor(timeSpan4), new TimeSpan(23, 00, 0));

            // Ceiling
            Assert.AreEqual(TimeRounderHelper.Ceiling(timeSpan1), new TimeSpan(1, 0, 0));
            Assert.AreEqual(TimeRounderHelper.Ceiling(timeSpan2), new TimeSpan(5, 0, 0));
            Assert.AreEqual(TimeRounderHelper.Ceiling(timeSpan3), new TimeSpan(9, 0, 0));
            Assert.AreEqual(TimeRounderHelper.Ceiling(timeSpan4), new TimeSpan(23, 59, 0));

            Assert.Pass();
        }

        [Test]
        public void ActivityRoundingTest()
        {
            // Original datetimes
            DateTime startAt1 = new(2021, 10, 16, 15, 44, 45);
            DateTime endAt2 = new(2021, 10, 16, 16, 2, 1);
            DateTime startAt3 = new(2021, 10, 16, 15, 0, 0);
            DateTime endAt3 = new(2021, 10, 16, 14, 44, 0);

            // Init activities
            Activity activity1 = new()
            {
                Name = "Test activity 1",
                CreatedAt = DateTime.Now,
                StartAt = startAt1
            };
            Activity activity2 = new()
            {
                Name = "Test activity 2",
                CreatedAt = DateTime.Now,
                EndAt = endAt2
            };
            Activity activity3 = new()
            {
                Name = "Test activity 3",
                CreatedAt = DateTime.Now,
                StartAt = startAt3,
                EndAt = endAt3
            };
            Assert.Greater(activity3.StartAt, activity3.EndAt);

            // Round
            TimeRounderHelper.Round(activity1);
            Assert.IsNull(activity1.EndAt);
            Assert.AreEqual(activity1.StartAt, new DateTime(2021, 10, 16, 16, 0, 0));

            TimeRounderHelper.Round(activity2);
            Assert.IsNull(activity2.StartAt);
            Assert.AreEqual(activity2.EndAt, new DateTime(2021, 10, 16, 16, 0, 0));

            TimeRounderHelper.Round(activity3);
            Assert.IsNotNull(activity3.StartAt);
            Assert.IsNotNull(activity3.EndAt);
            Assert.Greater(activity3.EndAt, activity3.StartAt);

            activity1.StartAt = startAt1;
            activity2.EndAt = endAt2;
            activity3.StartAt = startAt3;
            activity3.EndAt = endAt3;

            // Floor
            TimeRounderHelper.Floor(activity1);
            Assert.IsNull(activity1.EndAt);
            Assert.AreEqual(activity1.StartAt, new DateTime(2021, 10, 16, 15, 0, 0));

            TimeRounderHelper.Floor(activity2);
            Assert.IsNull(activity2.StartAt);
            Assert.AreEqual(activity2.EndAt, new DateTime(2021, 10, 16, 16, 0, 0));

            TimeRounderHelper.Floor(activity3);
            Assert.IsNotNull(activity3.StartAt);
            Assert.IsNotNull(activity3.EndAt);
            Assert.AreEqual(activity3.EndAt, activity3.StartAt);

            activity1.StartAt = startAt1;
            activity2.EndAt = endAt2;
            activity3.StartAt = startAt3;
            activity3.EndAt = endAt3;

            // Ceiling
            TimeRounderHelper.Ceiling(activity1);
            Assert.IsNull(activity1.EndAt);
            Assert.AreEqual(activity1.StartAt, new DateTime(2021, 10, 16, 16, 0, 0));

            TimeRounderHelper.Ceiling(activity2);
            Assert.IsNull(activity2.StartAt);
            Assert.AreEqual(activity2.EndAt, new DateTime(2021, 10, 16, 17, 0, 0));

            TimeRounderHelper.Ceiling(activity3);
            Assert.IsNotNull(activity3.StartAt);
            Assert.IsNotNull(activity3.EndAt);
            Assert.Greater(activity3.EndAt, activity3.StartAt);

            Assert.Pass();
        }
    }
}
