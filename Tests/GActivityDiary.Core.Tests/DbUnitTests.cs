using GActiveDiary.Tests.Common.DataBaseUtils;
using GActivityDiary.Core.DataBase;
using GActivityDiary.Core.Models;
using NHibernate;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GActivityDiary.Core.Tests
{
    public class DbUnitTests
    {
        private const string _testDBFilePath = "test.db";

        [SetUp]
        public void Setup()
        {
            if (File.Exists(_testDBFilePath))
            {
                File.Delete(_testDBFilePath);
            }
        }

        [TearDown]
        public void Cleanup()
        {
            if (File.Exists(_testDBFilePath))
            {
                File.Delete(_testDBFilePath);
            }
        }

        [Test]
        public void BasicCRUDTest()
        {
            DbContext dbContext = new(_testDBFilePath);
            var session = dbContext.Session;
            var activityRepository = dbContext.Activities;

            // 1. Read all

            var activities = activityRepository.GetAll();
            Assert.AreEqual(0, activities.Count);

            // 2. Create / Insert

            Activity activity1 = new()
            {
                Name = "Test Activity 1",
                CreatedAt = DateTime.Now
            };

            ITransaction transaction = session.BeginTransaction();
            activityRepository.Save(activity1);
            transaction.Commit();

            activities = activityRepository.GetAll();
            Assert.AreEqual(1, activities.Count);

            var firstActivity = activities[0];

            // 3. Update and read by Id

            firstActivity.Description = "Test 1";

            transaction = session.BeginTransaction();
            activityRepository.Save(firstActivity);
            transaction.Commit();

            activities = activityRepository.GetAll();
            Assert.AreEqual(1, activities.Count);

            firstActivity = activityRepository.GetById(firstActivity.Id);
            Assert.IsNotNull(firstActivity);
            Assert.AreSame(firstActivity.Description, "Test 1");

            // 4. Delete

            transaction = session.BeginTransaction();
            activityRepository.Delete(firstActivity);
            transaction.Commit();

            activities = activityRepository.GetAll();
            Assert.AreEqual(0, activities.Count);

            session.Close();

            Assert.Pass();
        }

        [Test]
        public void CRUDTest()
        {
            DbContext db = new(_testDBFilePath);
            //db.AutoRegisterRepositories();

            // 1. Read all

            var activities = db.Activities.GetAll();
            Assert.AreEqual(0, activities.Count);

            // 2. Create / Insert

            Activity activity1 = new()
            {
                Name = "Test Activity 1",
                CreatedAt = DateTime.Now
            };

            db.Activities.Save(activity1);

            activities = db.Activities.GetAll();
            Assert.AreEqual(1, activities.Count);

            // 3. Update and read by Id

            var firstActivity = activities[0];
            firstActivity.Description = "Test 1";

            db.Activities.Save(firstActivity);

            activities = db.Activities.GetAll();
            Assert.AreEqual(1, activities.Count);

            firstActivity = db.Activities.GetById(firstActivity.Id);
            Assert.IsNotNull(firstActivity);
            Assert.AreSame(firstActivity.Description, "Test 1");

            // 4. Delete

            db.Activities.Delete(firstActivity);

            activities = db.Activities.GetAll();
            Assert.AreEqual(0, activities.Count);

            Assert.Pass();
        }

        [Test]
        public void TransactionCRUDTest()
        {
            DbContext db = new(_testDBFilePath);
            //db.AutoRegisterRepositories();

            // 1. Read all

            var activities = db.Activities.GetAll();
            Assert.AreEqual(0, activities.Count);

            // 2. Create / Insert transaction

            var transaction = db.BeginTransaction();
            Activity activity1 = new()
            {
                Name = "Test Activity 1",
                CreatedAt = DateTime.Now
            };
            Activity activity2 = new()
            {
                Name = "Test Activity 2",
                CreatedAt = DateTime.Now
            };

            db.Activities.Save(activity1);
            db.Activities.Save(activity2);

            activities = db.Activities.GetAll();
            Assert.AreEqual(2, activities.Count);

            transaction.Rollback();

            activities = db.Activities.GetAll();
            Assert.AreEqual(0, activities.Count);

            Activity activity3 = new()
            {
                Name = "Test Activity 3",
                CreatedAt = DateTime.Now
            };
            Activity activity4 = new()
            {
                Name = "Test Activity 4",
                CreatedAt = DateTime.Now
            };

            transaction = db.BeginTransaction();
            db.Activities.Save(activity3);
            db.Activities.Save(activity4);
            db.Commit();

            activities = db.Activities.GetAll();
            Assert.AreEqual(2, activities.Count);
            Assert.True(transaction.WasCommitted, "Transaction was not committed");

            // 3. Update and read by Id

            var firstActivity = activities[0];
            firstActivity.Description = "Test 1";

            transaction = db.BeginTransaction();
            db.Activities.Save(firstActivity);
            transaction.Rollback();
            db.ResetSession();

            firstActivity = db.Activities.GetById(firstActivity.Id);
            Assert.True(string.IsNullOrEmpty(firstActivity.Description), "firstActivity.Description is not empty");

            // 4. Delete

            activities = db.Activities.GetAll();
            transaction = db.BeginTransaction();
            db.Activities.Delete(activities[0]);
            db.Activities.Delete(activities[1]);

            activities = db.Activities.GetAll();
            Assert.AreEqual(0, activities.Count);

            transaction.Rollback();

            activities = db.Activities.GetAll();
            Assert.AreEqual(2, activities.Count);

            db.Activities.Delete(activities[0]);
            db.Activities.Delete(activities[1]);
            db.Commit();

            activities = db.Activities.GetAll();
            Assert.AreEqual(0, activities.Count);

            Assert.Pass();
        }

        [Test]
        public void FilterTest()
        {
            DbContext db = new(_testDBFilePath);

            // 1. Read all

            var activities = db.Activities.GetAll();
            Assert.AreEqual(0, activities.Count);

            // 2. Create / Insert

            HashSet<Tag> tags = new()
            {
                new Tag("tag1"),
                new Tag("tag2"),
                new Tag("tag3")
            };

            ActivityType[] activityTypes = new[]
            {
                new ActivityType(),
                new ActivityType("type1"),
                new ActivityType("type2", 0),
                new ActivityType("type3", 199999.99m)
            };

            DateTime now = DateTime.Now;

            Activity activity1 = new()
            {
                Name = "Test Activity 1",
                ActivityType = activityTypes[1],
                CreatedAt = now
            };
            Activity activity2 = new()
            {
                Name = "Test Activity 2",
                ActivityType = activityTypes[2],
                CreatedAt = now.AddHours(-1),
                Tags = tags
            };
            Activity activity3 = new()
            {
                Name = "Test Activity 3",
                ActivityType = activityTypes[3],
                CreatedAt = now.AddHours(1)
            };

            activity3.Tags.Add(tags.ToArray()[0]);
            activity3.Tags.Add(new Tag("test"));

            db.BeginTransaction();
            db.Activities.Save(activity1);
            db.Activities.Save(activity2);
            db.Activities.Save(activity3);
            db.Commit();

            activities = db.Activities.GetAll();
            Assert.AreEqual(3, activities.Count);

            // 3. Find by criterion (eq)

            var query = db.Activities.Query();
            var queryActivities1 = query.Where(x => x.CreatedAt == now).ToList();
            var foundActivities2 = db.Activities.Find(x => x.CreatedAt == now);

            Assert.AreEqual(1, queryActivities1.Count);
            Assert.AreEqual(1, foundActivities2.Count);

            Assert.AreEqual(queryActivities1[0], activities[0]);
            Assert.AreEqual(foundActivities2[0], activities[0]);

            queryActivities1 = query.Where(x => x.Tags.Contains(tags.ToArray()[1])).ToList();
            foundActivities2 = db.Activities.Find(x => x.Tags.Contains(tags.ToArray()[1]));

            Assert.AreEqual(1, queryActivities1.Count);
            Assert.AreEqual(1, foundActivities2.Count);

            Assert.AreEqual(queryActivities1[0], activities[1]);
            Assert.AreEqual(foundActivities2[0], activities[1]);

            // 4. Find by criterion (last month)

            var endDate = now.AddSeconds(1);
            var beginDate = endDate.AddMonths(-1);

            query = db.Activities.Query();
            queryActivities1 = query.Where(x => x.CreatedAt >= beginDate && x.CreatedAt <= endDate).ToList();
            foundActivities2 = db.Activities.Find(x => x.CreatedAt >= beginDate && x.CreatedAt <= endDate);

            Assert.AreEqual(2, queryActivities1.Count);
            Assert.AreEqual(2, foundActivities2.Count);

            // 5. Tags

            var foundTags = db.Tags.Query().Where(x => x.Name == tags.ToArray()[0].Name).ToList();
            Assert.AreEqual(1, foundTags.Count);

            var activitiesWithTestTag = db.Activities.Find(x => x.Tags.Any(x => x.Name == "test")).ToList();
            Assert.AreEqual(1, activitiesWithTestTag.Count);

            activitiesWithTestTag = db.Activities.Find(x => x.Tags.Any(x => x.Name == tags.ToArray()[0].Name)).ToList();
            Assert.AreEqual(2, activitiesWithTestTag.Count);

            // 6. Activity Types

            foreach (var activityType in activityTypes)
            {
                var activity = db.Activities.Find(x => x.ActivityType!.Id == activityType.Id);
                Assert.IsNotNull(activity);
            }

            // 7. Delete

            db.BeginTransaction();
            db.Activities.Delete(activities[0]);
            db.Activities.Delete(activities[1]);
            db.Activities.Delete(activities[2]);
            db.Commit();

            activities = db.Activities.GetAll();
            Assert.AreEqual(0, activities.Count);

            Assert.Pass();
        }

        [Test]
        public void MassiveDBTest()
        {
            DbContext db = DataBaseGenerator.SimpleGenerate(_testDBFilePath, 1000000, 100000);

            Assert.IsNotNull(db);

            Assert.Pass();
        }

        [Test]
        public void PagingTest()
        {
            int activityCount = 1000;
            int pageIndex = 0;
            int pageSize = 10;
            int pageCount = activityCount / pageSize;

            DbContext db = DataBaseGenerator.SimpleGenerate(_testDBFilePath, activityCount);

            Assert.IsNotNull(db);

            for (; pageIndex < pageCount; pageIndex++)
            {
                var activities = db.Activities.GetAll(pageIndex, pageSize);
            }

            var emptyActivities = db.Activities.GetAll(pageIndex, pageSize);
            Assert.IsTrue(emptyActivities.Count == 0);

            Assert.Pass();
        }
    }
}
