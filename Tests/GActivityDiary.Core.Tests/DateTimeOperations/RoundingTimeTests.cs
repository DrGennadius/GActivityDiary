using GActivityDiary.Core.Helpers;
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

            Assert.AreEqual(TimeRounderHelper.Round(timeSpan1), new TimeSpan(1, 0, 0));
            Assert.AreEqual(TimeRounderHelper.Round(timeSpan2), new TimeSpan(4, 0, 0));
            Assert.AreEqual(TimeRounderHelper.Round(timeSpan3), new TimeSpan(9, 0, 0));
            Assert.AreEqual(TimeRounderHelper.Round(timeSpan4), new TimeSpan(23, 59, 0));

            Assert.AreEqual(TimeRounderHelper.Ceiling(timeSpan1), new TimeSpan(1, 0, 0));
            Assert.AreEqual(TimeRounderHelper.Ceiling(timeSpan2), new TimeSpan(5, 0, 0));
            Assert.AreEqual(TimeRounderHelper.Ceiling(timeSpan3), new TimeSpan(9, 0, 0));
            Assert.AreEqual(TimeRounderHelper.Ceiling(timeSpan4), new TimeSpan(23, 59, 0));

            Assert.AreEqual(TimeRounderHelper.Floor(timeSpan1), new TimeSpan(1, 0, 0));
            Assert.AreEqual(TimeRounderHelper.Floor(timeSpan2), new TimeSpan(4, 0, 0));
            Assert.AreEqual(TimeRounderHelper.Floor(timeSpan3), new TimeSpan(8, 0, 0));
            Assert.AreEqual(TimeRounderHelper.Floor(timeSpan4), new TimeSpan(23, 00, 0));

            Assert.Pass();
        }
    }
}
