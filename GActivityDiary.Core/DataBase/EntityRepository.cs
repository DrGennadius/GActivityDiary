using GActivityDiary.Core.Models;
using NHibernate;
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

        public async Task DeleteAsync(T item)
        {
            GetCurrentTransaction(out ITransaction transaction, out bool isNew);
            await _session.DeleteAsync(item);
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
            return new List<T>(_session.CreateCriteria(typeof(T)).List<T>());
        }

        public IList<T> GetAll(int pageIndex, int pageSize)
        {
            return new List<T>(_session
                .CreateCriteria(typeof(T))
                .SetFirstResult(pageIndex * pageSize)
                .SetMaxResults(pageSize)
                .List<T>());
        }

        public async Task<IList<T>> GetAllAsync()
        {
            return new List<T>(await _session.CreateCriteria(typeof(T)).ListAsync<T>());
        }

        public async Task<IList<T>> GetAllAsync(int pageIndex, int pageSize)
        {
            return new List<T>(await _session
                .CreateCriteria(typeof(T))
                .SetFirstResult(pageIndex * pageSize)
                .SetMaxResults(pageSize)
                .ListAsync<T>());
        }

        public T GetById(Guid id)
        {
            return _session.Get<T>(id);
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _session.GetAsync<T>(id);
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

        public IQueryable<T> Query()
        {
            return _session.Query<T>();
        }

        public Guid Save(T item)
        {
            GetCurrentTransaction(out ITransaction transaction, out bool isNew);
            Guid uid = (Guid)_session.Save(item);
            if (isNew)
            {
                transaction.Commit();
            }
            return uid;
        }

        public async Task<Guid> SaveAsync(T item)
        {
            GetCurrentTransaction(out ITransaction transaction, out bool isNew);
            Guid uid = (Guid)await _session.SaveAsync(item);
            if (isNew)
            {
                await transaction.CommitAsync();
            }
            return uid;
        }
    }
}
