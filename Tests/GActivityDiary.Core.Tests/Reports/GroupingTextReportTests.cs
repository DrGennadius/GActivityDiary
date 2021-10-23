using GActiveDiary.Tests.Common.DataBaseUtils;
using GActivityDiary.Core.Common;
using GActivityDiary.Core.DataBase;
using GActivityDiary.Core.Extensions;
using GActivityDiary.Core.Models;
using GActivityDiary.Core.Reports;
using GActivityDiary.Core.Reports.Text;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;

namespace GActivityDiary.Core.Tests.Reports
{
    public class GroupingTextReportTests
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
        public void DayGroupingTest()
        {
            LanguageProfile languageProfile = LanguageProfile.GetDefaultEng();
            SimpleTextReporter simpleTextReporter = new(_db, languageProfile);
            simpleTextReporter.GroupingType = ReportGroupingType.Day;

            // Activity
            IQueryable<Activity> activities = _db.Activities.Query();
            Activity lastActivity = activities.OrderByDescending(x => x.CreatedAt)
                                              .First();

            string activityReport = simpleTextReporter.GetReport(lastActivity);
            Assert.IsNotEmpty(activityReport);

            // Daily
            string dailyReport = simpleTextReporter.GetReport(DateTime.Now.AddDays(-1));
            Assert.IsNotEmpty(dailyReport);

            // Weekly
            string weeklyReport = simpleTextReporter.GetReport(DateTime.Now.AddDays(-7).GetWeekInterval());
            Assert.IsNotEmpty(weeklyReport);

            // Monthly
            string monthlyReport = simpleTextReporter.GetReport(DateTime.Now.AddMonths(-1).GetMonthInterval());
            Assert.IsNotEmpty(monthlyReport);

            // Quarterly
            string quarterlyReport = simpleTextReporter.GetReport(DateTime.Now.AddMonths(-3).GetQuarterInterval());
            Assert.IsNotEmpty(quarterlyReport);

            // Annual
            string annualReport = simpleTextReporter.GetReport(DateTime.Now.AddYears(-1).GetYearInterval());
            Assert.IsNotEmpty(annualReport);

            Assert.Pass();
        }
    }
}
