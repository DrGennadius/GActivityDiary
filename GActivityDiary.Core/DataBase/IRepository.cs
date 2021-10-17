using GActivityDiary.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GActivityDiary.Core.DataBase
{
    /// <summary>
    /// Repository of <see cref="IEntity"/>.
    /// </summary>
    /// <typeparam name="T">Entity</typeparam>
    public interface IRepository<T> where T : IEntity
    {
        /// <summary>
        /// Database context.
        /// </summary>
        IDbContext DbContext { get; }

        /// <summary>
        /// Save the entry to db. Return id.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Guid Save(T item);

        /// <summary>
        /// Save the entry to db. Return id.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<Guid> SaveAsync(T item);

        T GetById(Guid id);

        Task<T> GetByIdAsync(Guid id);

        /// <summary>
        /// Retrieve all entries. Dont recommended for use in real cases.
        /// </summary>
        /// <returns></returns>
        IList<T> GetAll();

        /// <summary>
        /// Retrieve all entries for single page.
        /// </summary>
        /// <param name="pageIndex">Page index.</param>
        /// <param name="pageSize">Page size.</param>
        /// <returns></returns>
        IList<T> GetAll(int pageIndex, int pageSize);

        /// <summary>
        /// Retrieve all entries. Dont recommended for use in real cases.
        /// </summary>
        /// <returns></returns>
        Task<IList<T>> GetAllAsync();

        /// <summary>
        /// Retrieve all entries for single page.
        /// </summary>
        /// <param name="pageIndex">Page index.</param>
        /// <param name="pageSize">Page size.</param>
        /// <returns></returns>
        Task<IList<T>> GetAllAsync(int pageIndex, int pageSize);

        IQueryable<T> Query();

        /// <summary>
        /// Find by expression.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IList<T> Find(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Find by expression.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<IList<T>> FindAsync(Expression<Func<T, bool>> predicate);

        void Delete(T item);

        Task DeleteAsync(T item);
    }
}
