using GActivityDiary.Core.Models;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GActivityDiary.Core.DataBase
{
    public interface IRepository
    {
        void GetCurrentTransaction(out ITransaction transaction, out bool isNew);
    }

    public interface IRepository<T> : IRepository where T : IEntity
    {
        void Save(T item);

        Task SaveAsync(T item);

        T GetById(Guid id);

        Task<T> GetByIdAsync(Guid id);

        IList<T> GetAll();

        Task<IList<T>> GetAllAsync();

        Task<IList<T>> GetAllAsync(int pageIndex, int pageSize);

        IQueryable<T> Query();

        IList<T> Find(Expression<Func<T, bool>> predicate);

        Task<IList<T>> FindAsync(Expression<Func<T, bool>> predicate);

        void Delete(T item);

        Task DeleteAsync(T item);
    }
}
