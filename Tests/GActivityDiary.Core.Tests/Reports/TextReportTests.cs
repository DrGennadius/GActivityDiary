using GActiveDiary.Tests.Common.DataBaseUtils;
using GActivityDiary.Core.Common;
using GActivityDiary.Core.DataBase;
using GActivityDiary.Core.Models;
using GActivityDiary.Core.Reports.Text;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;

namespace GActivityDiary.Core.Tests.Reports
{
    public class TextReportTests
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
            _db = DataBaseGenerator.Generate(beginDateTime: now.AddYears(-3),
                                             endDateTime: now,
                                             dbFilePath: _testDBFilePath,
                                             activitiesPerDay: 48);
        }

        [Test]
        public void ActivityTest()
        {
            LanguageProfile languageProfile = LanguageProfile.GetDefaultEng();
            SimpleTextReporter simpleTextReporter = new(_db, languageProfile);

            IQueryable<Activity> activities = _db.Activities.Query();
            Activity lastActivity = activities.OrderByDescending(x => x.CreatedAt)
                                              .First();

            string activityReport = simpleTextReporter.GetReport(lastActivity);
            Assert.IsNotEmpty(activityReport);

            Assert.Pass();
        }

        [Test]
        public void DailyTest()
        {
            LanguageProfile languageProfile = LanguageProfile.GetDefaultEng();
            SimpleTextReporter simpleTextReporter = new(_db, languageProfile);

            string dailyReport = simpleTextReporter.GetReport(DateTime.Now.AddDays(-1));
            Assert.IsNotEmpty(dailyReport);

            Assert.Pass();
        }

        [Test]
        public void WeeklyTest()
        {
            LanguageProfile languageProfile = LanguageProfile.GetDefaultEng();
            SimpleTextReporter simpleTextReporter = new(_db, languageProfile);

            var dateTime = DateTime.Now.AddDays(-7);
            int dayOfWeek = (int)dateTime.DayOfWeek;
            dateTime = dateTime.AddDays( - dayOfWeek + 1);
            DateTime beginDateTime = new(dateTime.Year, dateTime.Month, dateTime.Day);
            DateTime endDateTime = beginDateTime.AddDays(7)
                                                .AddMilliseconds(-1);
            string weeklyReport = simpleTextReporter.GetReport(beginDateTime,
                                                               endDateTime);
            Assert.IsNotEmpty(weeklyReport);

            Assert.Pass();
        }

        [Test]
        public void WorkWeeklyTest()
        {
            // Generate a work weekly report as text.

            Assert.Pass();
        }

        [Test]
        public void QuarterlyTest()
        {
            LanguageProfile languageProfile = LanguageProfile.GetDefaultEng();
            SimpleTextReporter simpleTextReporter = new(_db, languageProfile);

            var dateTime = DateTime.Now.AddDays(-1);
            int year = dateTime.Year;
            int month = dateTime.Month;
            int quarter = (month + 2) / 3;
            if (quarter == 1)
            {
                year--;
                month = 10;
            }
            else
            {
                month = (quarter - 1) * 3 - 2;
            }
            DateTime beginDateTime = new(year, month, 1);
            DateTime endDateTime = beginDateTime.AddMonths(3)
                                                .AddMilliseconds(-1);
            string quarterlyReport = simpleTextReporter.GetReport(beginDateTime,
                                                                  endDateTime);
            Assert.IsNotEmpty(quarterlyReport);

            Assert.Pass();
        }

        [Test]
        public void AnnualTest()
        {
            LanguageProfile languageProfile = LanguageProfile.GetDefaultEng();
            SimpleTextReporter simpleTextReporter = new(_db, languageProfile);

            int year = DateTime.Now.Year - 1;
            DateTime beginDateTime = new(year, 1, 1);
            DateTime endDateTime = beginDateTime.AddYears(1)
                                                .AddMilliseconds(-1);
            string annualReport = simpleTextReporter.GetReport(beginDateTime,
                                                               endDateTime);
            Assert.IsNotEmpty(annualReport);

            Assert.Pass();
        }
    }
}
