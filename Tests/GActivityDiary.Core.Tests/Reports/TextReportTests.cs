using GActiveDiary.Tests.Common.DataBaseUtils;
using GActivityDiary.Core.Common;
using GActivityDiary.Core.DataBase;
using GActivityDiary.Core.Extensions;
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

            string weeklyReport = simpleTextReporter.GetReport(DateTime.Now.AddDays(-7).GetWeekInterval());
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
        public void MonthlyTest()
        {
            LanguageProfile languageProfile = LanguageProfile.GetDefaultEng();
            SimpleTextReporter simpleTextReporter = new(_db, languageProfile);

            string monthlyReport = simpleTextReporter.GetReport(DateTime.Now.AddMonths(-1).GetMonthInterval());
            Assert.IsNotEmpty(monthlyReport);

            Assert.Pass();
        }

        [Test]
        public void QuarterlyTest()
        {
            LanguageProfile languageProfile = LanguageProfile.GetDefaultEng();
            SimpleTextReporter simpleTextReporter = new(_db, languageProfile);

            string quarterlyReport = simpleTextReporter.GetReport(DateTime.Now.AddMonths(-3).GetQuarterInterval());
            Assert.IsNotEmpty(quarterlyReport);

            Assert.Pass();
        }

        [Test]
        public void AnnualTest()
        {
            LanguageProfile languageProfile = LanguageProfile.GetDefaultEng();
            SimpleTextReporter simpleTextReporter = new(_db, languageProfile);

            string annualReport = simpleTextReporter.GetReport(DateTime.Now.AddYears(-1).GetYearInterval());
            Assert.IsNotEmpty(annualReport);

            Assert.Pass();
        }
    }
}
