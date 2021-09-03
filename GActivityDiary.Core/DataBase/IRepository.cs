using GActivityDiary.Core.Models;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace GActivityDiary.Core.DataBase
{
    public interface IRepository
    {
        void GetCurrentTransaction(out ITransaction transaction, out bool isNew);
    }

    public interface IRepository<T> : IRepository where T : IEntity
    {
        void Save(T item);

        T GetById(Guid id);

        IList<T> GetAll();

        IQueryable<T> Query();

        IList<T> Find(Expression<Func<T, bool>> predicate);

        void Delete(T item);
    }
}
