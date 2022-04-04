using GActivityDiary.Core.Common;
using NUnit.Framework;
using System;

namespace GActivityDiary.Core.Tests.DateTimeOperations
{
    public class DateTimeTests
    {
        [Test]
        public void DateTimeIntervalIntersection()
        {
            DateTime today = DateTime.Today;
            DateTime yesterday = today.AddDays(-1);

            DateTimeInterval todayInterval = new(today);
            DateTimeInterval yesterdayInterval = new(yesterday);

            Assert.IsFalse(todayInterval.IsIntersected(yesterdayInterval));
            Assert.IsFalse(yesterdayInterval.IsIntersected(todayInterval));

            DateTimeInterval yesterdayAndTodayInterval = DateTimeInterval.CreateFromDays(yesterday, today);

            Assert.AreEqual(2, (yesterdayAndTodayInterval.End - yesterdayAndTodayInterval.Start).TotalDays);
            Assert.IsTrue(todayInterval.IsIntersected(yesterdayAndTodayInterval));
            Assert.IsTrue(yesterdayInterval.IsIntersected(yesterdayAndTodayInterval));
            Assert.IsTrue(yesterdayAndTodayInterval.IsIntersected(todayInterval));
            Assert.IsTrue(yesterdayAndTodayInterval.IsIntersected(yesterdayInterval));

            DateTimeInterval halfYesterdayAndHalfTodayInterval = new(yesterday.AddHours(12), today.AddHours(12));

            Assert.AreEqual(24, (halfYesterdayAndHalfTodayInterval.End - halfYesterdayAndHalfTodayInterval.Start).TotalHours);
            Assert.IsTrue(todayInterval.IsIntersected(halfYesterdayAndHalfTodayInterval));
            Assert.IsTrue(yesterdayInterval.IsIntersected(halfYesterdayAndHalfTodayInterval));
            Assert.IsTrue(halfYesterdayAndHalfTodayInterval.IsIntersected(todayInterval));
            Assert.IsTrue(halfYesterdayAndHalfTodayInterval.IsIntersected(yesterdayInterval));

            Assert.Pass();
        }

        [Test]
        public void DateTimeIntervalWithin()
        {
            DateTime today = DateTime.Today;
            DateTime yesterday = today.AddDays(-1);

            DateTimeInterval todayInterval = new(today);
            DateTimeInterval yesterdayInterval = new(yesterday);

            Assert.IsFalse(todayInterval.IsWithin(yesterdayInterval));
            Assert.IsFalse(yesterdayInterval.IsWithin(todayInterval));

            DateTimeInterval yesterdayAndTodayInterval = DateTimeInterval.CreateFromDays(yesterday, today);

            Assert.AreEqual(2, (yesterdayAndTodayInterval.End - yesterdayAndTodayInterval.Start).TotalDays);
            Assert.IsTrue(todayInterval.IsWithin(yesterdayAndTodayInterval));
            Assert.IsTrue(yesterdayInterval.IsWithin(yesterdayAndTodayInterval));
            Assert.IsFalse(yesterdayAndTodayInterval.IsWithin(todayInterval));
            Assert.IsFalse(yesterdayAndTodayInterval.IsWithin(yesterdayInterval));

            DateTimeInterval halfYesterdayAndHalfTodayInterval = new(yesterday.AddHours(12), today.AddHours(12));

            Assert.AreEqual(24, (halfYesterdayAndHalfTodayInterval.End - halfYesterdayAndHalfTodayInterval.Start).TotalHours);
            Assert.IsTrue(halfYesterdayAndHalfTodayInterval.IsWithin(yesterdayAndTodayInterval));
            Assert.IsFalse(todayInterval.IsWithin(halfYesterdayAndHalfTodayInterval));
            Assert.IsFalse(yesterdayInterval.IsWithin(halfYesterdayAndHalfTodayInterval));
            Assert.IsFalse(halfYesterdayAndHalfTodayInterval.IsWithin(todayInterval));
            Assert.IsFalse(halfYesterdayAndHalfTodayInterval.IsWithin(yesterdayInterval));

            Assert.Pass();
        }
    }
}
