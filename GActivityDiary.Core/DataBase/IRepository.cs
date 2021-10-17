using GActivityDiary.Core.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GActivityDiary.Core.DataBase
{
    /// <summary>
    /// Repository.
    /// </summary>
    public interface IRepository
    {
        void GetCurrentTransaction(out ITransaction transaction, out bool isNew);
    }

    /// <summary>
    /// Repository of <see cref="IEntity"/>.
    /// </summary>
    /// <typeparam name="T">Entity</typeparam>
    public interface IRepository<T> : IRepository where T : IEntity
    {
        Guid Save(T item);

        Task<Guid> SaveAsync(T item);

        T GetById(Guid id);

        Task<T> GetByIdAsync(Guid id);

        IList<T> GetAll();

        IList<T> GetAll(int pageIndex, int pageSize);

        Task<IList<T>> GetAllAsync();

        Task<IList<T>> GetAllAsync(int pageIndex, int pageSize);

        IQueryable<T> Query();

        IList<T> Find(Expression<Func<T, bool>> predicate);

        Task<IList<T>> FindAsync(Expression<Func<T, bool>> predicate);

        void Delete(T item);

        Task DeleteAsync(T item);
    }
}
