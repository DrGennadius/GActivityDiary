using GActiveDiary.Tests.Common.DataBaseUtils;
using GActivityDiary.Core.DataBase;
using GActivityDiary.Core.Helpers;
using GActivityDiary.Core.Models;
using NHibernate;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GActivityDiary.Core.Tests.Searching
{
    public class ActivitySearchingTests
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
                                             endDateTime: now,
                                             dbFilePath: _testDBFilePath,
                                             activitiesPerDay: 4);
        }

        [Test]
        public async Task SearchByTagStringsTestAsync()
        {
            Tag originTag1 = new("tag1");
            Tag originTag2 = new("tag2");
            Tag originTag3 = new("tag3");

            Activity originActivity1 = new("Activity 1");
            Activity originActivity2 = new("Activity 2");
            Activity originActivity3 = new("Activity 3");

            originActivity1.Tags.Add(originTag1);

            originActivity2.Tags.Add(originTag1);
            originActivity2.Tags.Add(originTag2);

            originActivity3.Tags.Add(originTag1);
            originActivity3.Tags.Add(originTag2);
            originActivity3.Tags.Add(originTag3);

            ITransaction transaction = _db.BeginTransaction();
            _db.Tags.Save(originTag1);
            _db.Tags.Save(originTag2);
            _db.Tags.Save(originTag3);
            _db.Activities.Save(originActivity1);
            _db.Activities.Save(originActivity2);
            _db.Activities.Save(originActivity3);
            transaction.Commit();

            var activities = await ActivityHelper.GetByTagsAsync(_db, "tag3");
            Assert.AreEqual(activities.Count(), 1);

            activities = await ActivityHelper.GetByTagsAsync(_db, "tag2");
            Assert.AreEqual(activities.Count(), 2);

            activities = await ActivityHelper.GetByTagsAsync(_db, "tag1");
            Assert.AreEqual(activities.Count(), 3);

            Assert.Pass();
        }
    }
}
