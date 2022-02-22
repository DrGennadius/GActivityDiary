using GActivityDiary.Core.Models;
using GActivityDiary.Core.NHibernate;
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

        public EntityRepository<ActivityType> ActivityTypes { get; set; }

        //private readonly Dictionary<string, IRepository> _repositories;

        public DbContext(string dataBaseFilePath)
        {
            _dataBaseFilePath = dataBaseFilePath;
            Initialize();
        }

        public DbContext()
        {
            Initialize();
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

        private void Initialize()
        {
            NHibernateFactory nHibernateFactory = new(_dataBaseFilePath);
            NHibernateFactoryProxy nHibernateFactoryProxy = new();
            nHibernateFactoryProxy.Initialize(nHibernateFactory.Configuration, nHibernateFactory.SessionFactory);
            NHibernateSession nHibernateSession = new(nHibernateFactoryProxy);
            Session = nHibernateSession.OpenSession();
            Session.FlushMode = FlushMode.Auto;

            Activities = new EntityRepository<Activity>(this);
            Tags = new EntityRepository<Tag>(this);
            ActivityTypes = new EntityRepository<ActivityType>(this);
        }
    }
}
