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
    public class TagSearchingTests
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
            ITransaction transaction = _db.BeginTransaction();
            for (int i = 1; i <= 999; i++)
            {
                Tag newTag = new($"tag{i}");
                _db.Tags.Save(newTag);
            }
            transaction.Commit();

            var tags = await TagHelper.GetTagsAsync(_db, "tag55");
            Assert.AreEqual(1, tags.Count());

            var tagIds = await TagHelper.GetTagIdsAsync(_db, "tag55");
            Assert.AreEqual(1, tags.Count());

            Assert.AreEqual(tagIds.First(), tags.First().Id);

            Assert.Pass();
        }
    }
}
