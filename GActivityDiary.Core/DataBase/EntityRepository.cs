using GActivityDiary.Core.Models;
using NHibernate.Linq;
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
    /// <typeparam name="T"></typeparam>
    public class EntityRepository<T> : IRepository<T> where T : IEntity
    {
        public EntityRepository(IDbContext dbContext)
        {
            DbContext = dbContext;
        }

        /// <summary>
        /// Database context.
        /// </summary>
        public IDbContext DbContext { get; private set; }

        public void Delete(T item)
        {
            var (transaction, isNew) = DbContext.GetCurrentTransactionOrCreateNew();
            DbContext.Session.Delete(item);
            if (isNew)
            {
                transaction.Commit();
            }
        }

        public async Task DeleteAsync(T item)
        {
            var (transaction, isNew) = DbContext.GetCurrentTransactionOrCreateNew();
            await DbContext.Session.DeleteAsync(item);
            if (isNew)
            {
                await transaction.CommitAsync();
            }
        }

        public IList<T> Find(Expression<Func<T, bool>> predicate)
        {
            return Query().Where(predicate).ToList();
        }

        public async Task<IList<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await Query().Where(predicate).ToListAsync();
        }

        public IList<T> GetAll()
        {
            return new List<T>(DbContext.Session.CreateCriteria(typeof(T)).List<T>());
        }

        public IList<T> GetAll(int pageIndex, int pageSize)
        {
            return new List<T>(DbContext.Session
                .CreateCriteria(typeof(T))
                .SetFirstResult(pageIndex * pageSize)
                .SetMaxResults(pageSize)
                .List<T>());
        }

        public async Task<IList<T>> GetAllAsync()
        {
            return new List<T>(await DbContext.Session.CreateCriteria(typeof(T)).ListAsync<T>());
        }

        public async Task<IList<T>> GetAllAsync(int pageIndex, int pageSize)
        {
            return new List<T>(await DbContext.Session
                .CreateCriteria(typeof(T))
                .SetFirstResult(pageIndex * pageSize)
                .SetMaxResults(pageSize)
                .ListAsync<T>());
        }

        public T GetById(Guid id)
        {
            return DbContext.Session.Get<T>(id);
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await DbContext.Session.GetAsync<T>(id);
        }

        public IQueryable<T> Query()
        {
            return DbContext.Session.Query<T>();
        }

        public Guid Save(T item)
        {
            var (transaction, isNew) = DbContext.GetCurrentTransactionOrCreateNew();
            Guid uid = (Guid)DbContext.Session.Save(item);
            if (isNew)
            {
                transaction.Commit();
            }
            return uid;
        }

        public async Task<Guid> SaveAsync(T item)
        {
            var (transaction, isNew) = DbContext.GetCurrentTransactionOrCreateNew();
            Guid uid = (Guid)await DbContext.Session.SaveAsync(item);
            if (isNew)
            {
                await transaction.CommitAsync();
            }
            return uid;
        }
    }
}
