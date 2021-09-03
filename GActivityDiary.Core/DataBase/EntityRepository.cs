﻿using GActivityDiary.Core.Models;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace GActivityDiary.Core.DataBase
{
    public class EntityRepository<T> : IRepository<T> where T : IEntity
    {
        private ISession _session;

        public EntityRepository(ISession session)
        {
            _session = session;
        }

        public void Delete(T item)
        {
            GetCurrentTransaction(out ITransaction transaction, out bool isNew);
            _session.Delete(item);
            if (isNew)
            {
                transaction.Commit();
            }
        }

        public IList<T> GetAll()
        {
            return new List<T>(_session.CreateCriteria(typeof(T)).List<T>());
        }

        public IQueryable<T> Query()
        {
            return _session.Query<T>();
        }

        public IList<T> Find(Expression<Func<T, bool>> predicate)
        {
            return Query().Where(predicate).ToList();
        }

        public T GetById(Guid id)
        {
            return _session.Get<T>(id);
        }

        public void Save(T item)
        {
            GetCurrentTransaction(out ITransaction transaction, out bool isNew);
            _session.Save(item);
            if (isNew)
            {
                transaction.Commit();
            }
        }

        public void GetCurrentTransaction(out ITransaction transaction, out bool isNew)
        {
            transaction = _session.GetCurrentTransaction();
            isNew = transaction == null || !transaction.IsActive;
            if (isNew)
            {
                transaction = _session.BeginTransaction();
            }
        }
    }
}
