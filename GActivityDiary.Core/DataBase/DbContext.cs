using GActivityDiary.Core.Models;
using NHibernate;
using System;

namespace GActivityDiary.Core.DataBase
{
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
            //_repositories = new Dictionary<string, IRepository>();
        }

        public DbContext()
        {
            NHibernateHelper helper = new(_dataBaseFilePath);
            Session = helper.OpenSession();
            Activities = new EntityRepository<Activity>(Session);
            //_repositories = new Dictionary<string, IRepository>();
        }

        //public void AutoRegisterRepositories()
        //{
        //    Type genericType = typeof(EntityRepository<>);
        //    Type type = typeof(IEntity);
        //    var types = AppDomain.CurrentDomain.GetAssemblies()
        //        .SelectMany(s => s.GetTypes())
        //        .Where(p => type.IsAssignableFrom(p) && p.IsClass);
        //    foreach (var item in types)
        //    {
        //        IRepository repository = (IRepository)Activator.CreateInstance(genericType.MakeGenericType(item), _session);
        //        Register(repository);
        //    }
        //}

        //public void Register(IRepository repository)
        //{
        //    _repositories.Add(repository.GetType().Name, repository);
        //}

        //public void Unregister(IRepository repository)
        //{
        //    string key = repository.GetType().Name;
        //    if (_repositories.ContainsKey(key))
        //    {
        //        _repositories.Remove(key);
        //    }
        //}

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

        public void Dispose()
        {
            Session.Dispose();
        }
    }
}
