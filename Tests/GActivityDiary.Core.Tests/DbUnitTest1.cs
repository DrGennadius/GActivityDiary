using GActivityDiary.Core.DataBase;
using GActivityDiary.Core.Models;
using NHibernate;
using NHibernate.Criterion;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GActivityDiary.Core.Tests
{
    public class Tests
    {
        private const string _testDBFilePath = "test.db";

        private NHibernateHelper _nHibernateHelper;

        [SetUp]
        public void Setup()
        {
            if (File.Exists(_testDBFilePath))
            {
                File.Delete(_testDBFilePath);
            }
            _nHibernateHelper = new(_testDBFilePath);
        }

        [Test]
        public void BasicCRUDTest()
        {
            ISession session = _nHibernateHelper.OpenSession();

            EntityRepository<Activity> _activityRepository = new EntityRepository<Activity>(session);

            // 1. Read all

            var activities = _activityRepository.GetAll();
            Assert.True(activities.Count == 0, "Activity List is not empty");

            // 2. Create / Insert

            Activity activity1 = new()
            {
                Name = "Test Activity 1",
                CreatedAt = DateTime.Now
            };

            ITransaction transaction = session.BeginTransaction();
            _activityRepository.Save(activity1);
            transaction.Commit();

            activities = _activityRepository.GetAll();
            Assert.True(activities.Count == 1, "Activity Length is not 1");

            var firstActivity = activities[0];

            // 3. Update and read by Id

            firstActivity.Description = "Test 1";

            transaction = session.BeginTransaction();
            _activityRepository.Save(firstActivity);
            transaction.Commit();

            activities = _activityRepository.GetAll();
            Assert.True(activities.Count == 1, "Activity Length is not 1");

            firstActivity = _activityRepository.GetById(firstActivity.Id);
            Assert.IsNotNull(firstActivity);
            Assert.AreSame(firstActivity.Description, "Test 1");

            // 4. Delete

            transaction = session.BeginTransaction();
            _activityRepository.Delete(firstActivity);
            transaction.Commit();

            activities = _activityRepository.GetAll();
            Assert.True(activities.Count == 0, "Activity List is not empty");

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
            Assert.True(activities.Count == 0, "Activity List is not empty");

            // 2. Create / Insert

            Activity activity1 = new()
            {
                Name = "Test Activity 1",
                CreatedAt = DateTime.Now
            };

            db.Activities.Save(activity1);

            activities = db.Activities.GetAll();
            Assert.True(activities.Count == 1, "Activity Length is not 1");

            // 3. Update and read by Id

            var firstActivity = activities[0];
            firstActivity.Description = "Test 1";

            db.Activities.Save(firstActivity);

            activities = db.Activities.GetAll();
            Assert.True(activities.Count == 1, "Activity Length is not 1");

            firstActivity = db.Activities.GetById(firstActivity.Id);
            Assert.IsNotNull(firstActivity);
            Assert.AreSame(firstActivity.Description, "Test 1");

            // 4. Delete

            db.Activities.Delete(firstActivity);

            activities = db.Activities.GetAll();
            Assert.True(activities.Count == 0, "Activity List is not empty");

            Assert.Pass();
        }

        [Test]
        public void TransactionCRUDTest()
        {
            DbContext db = new(_testDBFilePath);
            //db.AutoRegisterRepositories();

            // 1. Read all

            var activities = db.Activities.GetAll();
            Assert.True(activities.Count == 0, "Activity List is not empty");

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
            Assert.True(activities.Count == 2, "Activity Length is not 2");

            transaction.Rollback();

            activities = db.Activities.GetAll();
            Assert.True(activities.Count == 0, "Activity List is not empty");

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
            Assert.True(activities.Count == 2, "Activity Length is not 2");
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
            Assert.True(activities.Count == 0, "Activity List is not empty");

            transaction.Rollback();

            activities = db.Activities.GetAll();
            Assert.True(activities.Count == 2, "Activity List is not 2");

            db.Activities.Delete(activities[0]);
            db.Activities.Delete(activities[1]);
            db.Commit();

            activities = db.Activities.GetAll();
            Assert.True(activities.Count == 0, "Activity List is not empty");

            Assert.Pass();
        }

        [Test]
        public void FilterTest()
        {
            DbContext db = new(_testDBFilePath);

            // 1. Read all

            var activities = db.Activities.GetAll();
            Assert.True(activities.Count == 0, "Activity List is not empty");

            // 2. Create / Insert

            HashSet<Tag> tags = new()
            {
                new Tag("tag1"),
                new Tag("tag2"),
                new Tag("tag3")
            };

            DateTime now = DateTime.Now;

            Activity activity1 = new()
            {
                Name = "Test Activity 1",
                CreatedAt = now
            };
            Activity activity2 = new()
            {
                Name = "Test Activity 2",
                CreatedAt = now.AddHours(-1),
                Tags = tags
            };
            Activity activity3 = new()
            {
                Name = "Test Activity 3",
                CreatedAt = now.AddHours(1)
            };

            activity3.Tags.Add(new Tag("test"));
            bool d = activity2.Tags.ToArray()[0].Equals(activity2.Tags.ToArray()[1]);

            db.BeginTransaction();
            db.Activities.Save(activity1);
            db.Activities.Save(activity2);
            db.Activities.Save(activity3);
            db.Commit();

            activities = db.Activities.GetAll();
            Assert.True(activities.Count == 3, "Activity Length is not 3");

            // 3. Find by criterion (eq)

            var query = db.Activities.Query();
            var queryActivities1 = query.Where(x => x.CreatedAt == now).ToList();
            var foundActivities2 = db.Activities.Find(x => x.CreatedAt == now);

            Assert.True(queryActivities1.Count == 1, "Activity list 2 length is not 1");
            Assert.True(foundActivities2.Count == 1, "Activity list 3 length is not 1");

            Assert.AreEqual(activities[0], queryActivities1[0]);
            Assert.AreEqual(activities[0], foundActivities2[0]);

            queryActivities1 = query.Where(x => x.Tags.Contains(tags.ToArray()[1])).ToList();
            foundActivities2 = db.Activities.Find(x => x.Tags.Contains(tags.ToArray()[1]));

            Assert.True(queryActivities1.Count == 1, "Activity list 2 length is not 1");
            Assert.True(foundActivities2.Count == 1, "Activity list 3 length is not 1");

            Assert.AreEqual(activities[1], queryActivities1[0]);
            Assert.AreEqual(activities[1], foundActivities2[0]);

            // 4. Find by criterion (last month)

            var endDate = now.AddSeconds(1);
            var beginDate = endDate.AddMonths(-1);

            query = db.Activities.Query();
            queryActivities1 = query.Where(x => x.CreatedAt >= beginDate && x.CreatedAt <= endDate).ToList();
            foundActivities2 = db.Activities.Find(x => x.CreatedAt >= beginDate && x.CreatedAt <= endDate);

            Assert.True(queryActivities1.Count == 2, "Activity list 2 length is not 2");
            Assert.True(foundActivities2.Count == 2, "Activity list 3 length is not 2");

            // 5. Delete

            db.BeginTransaction();
            db.Activities.Delete(activities[0]);
            db.Activities.Delete(activities[1]);
            db.Activities.Delete(activities[2]);
            db.Commit();

            activities = db.Activities.GetAll();
            Assert.True(activities.Count == 0, "Activity List is not empty");

            Assert.Pass();
        }
    }
}
