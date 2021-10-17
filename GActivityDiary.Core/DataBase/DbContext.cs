using GActivityDiary.Core.Helpers;
using GActivityDiary.Core.Models;
using NHibernate;
using System;
using System.Threading.Tasks;

namespace GActivityDiary.Core.DataBase
{
    /// <summary>
    /// Database Context (UoW)
    /// </summary>
    public class DbContext : IDbContext
    {
        private readonly string _dataBaseFilePath = "ActivityDiary.db";

        public ISession Session { get; private set; }

        public EntityRepository<Activity> Activities { get; private set; }

        public EntityRepository<Tag> Tags { get; private set; }

        //private readonly Dictionary<string, IRepository> _repositories;

        public DbContext(string dataBaseFilePath)
        {
            _dataBaseFilePath = dataBaseFilePath;
            NHibernateHelper helper = new(_dataBaseFilePath);
            Session = helper.OpenSession();
            Activities = new EntityRepository<Activity>(this);
            Tags = new EntityRepository<Tag>(this);
        }

        public DbContext()
        {
            NHibernateHelper helper = new(_dataBaseFilePath);
            Session = helper.OpenSession();
            Activities = new EntityRepository<Activity>(this);
            Tags = new EntityRepository<Tag>(this);
        }

        public ITransaction BeginTransaction()
        {
            return Session.BeginTransaction();
        }

        public (ITransaction, bool) GetCurrentTransactionOrCreateNew()
        {
            ITransaction transaction = Session.GetCurrentTransaction();
            bool isNew = transaction == null || !transaction.IsActive;
            if (isNew)
            {
                transaction = Session.BeginTransaction();
            }
            return (transaction, isNew);
        }

        public void Commit()
        {
            var transaction = Session.GetCurrentTransaction();
            if (transaction != null && transaction.IsActive)
            {
                transaction.Commit();
            }
        }

        public async Task CommitAsync()
        {
            var transaction = Session.GetCurrentTransaction();
            if (transaction != null && transaction.IsActive)
            {
                await transaction.CommitAsync();
            }
        }

        public void ResetSession()
        {
            Session.Disconnect();
            Session.Clear();
            Session.Reconnect();
        }

        public void Rollback()
        {
            var transaction = Session.GetCurrentTransaction();
            if (transaction != null && transaction.IsActive)
            {
                transaction.Rollback();
            }
        }

        public async Task RollbackAsync()
        {
            var transaction = Session.GetCurrentTransaction();
            if (transaction != null && transaction.IsActive)
            {
                await transaction.RollbackAsync();
            }
        }

        public void Dispose()
        {
            Session.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
