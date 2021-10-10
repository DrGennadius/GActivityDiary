using GActivityDiary.Core.Helpers;
using GActivityDiary.Core.Models;
using NHibernate;
using System.Threading.Tasks;

namespace GActivityDiary.Core.DataBase
{
    /// <summary>
    /// Database Context (UoW)
    /// </summary>
    public class DbContext : IDbContext
    {
        private string _dataBaseFilePath = "ActivityDiary.db";

        public ISession Session { get; private set; }

        public EntityRepository<Activity> Activities { get; private set; }

        public EntityRepository<Tag> Tags { get; private set; }

        //private readonly Dictionary<string, IRepository> _repositories;

        public DbContext(string dataBaseFilePath)
        {
            _dataBaseFilePath = dataBaseFilePath;
            NHibernateHelper helper = new(_dataBaseFilePath);
            Session = helper.OpenSession();
            Activities = new EntityRepository<Activity>(Session);
            Tags = new EntityRepository<Tag>(Session);
        }

        public DbContext()
        {
            NHibernateHelper helper = new(_dataBaseFilePath);
            Session = helper.OpenSession();
            Activities = new EntityRepository<Activity>(Session);
            Tags = new EntityRepository<Tag>(Session);
        }

        public ITransaction BeginTransaction()
        {
            return Session.BeginTransaction();
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
        }
    }
}
