using NHibernate;
using System;
using System.Threading.Tasks;

namespace GActivityDiary.Core.DataBase
{
    /// <summary>
    /// Database Context (UoW)
    /// </summary>
    public interface IDbContext : IDisposable
    {
        /// <summary>
        /// Session.
        /// </summary>
        ISession Session { get; }

        /// <summary>
        /// Begin session transaction.
        /// </summary>
        /// <returns></returns>
        ITransaction BeginTransaction();

        /// <summary>
        /// Get current an active transaction or create new if not exists.
        /// </summary>
        /// <returns></returns>
        public (ITransaction, bool) GetCurrentTransactionOrCreateNew();

        /// <summary>
        /// Flush the associated session and end the unit of work.
        /// </summary>
        void Commit();

        /// <summary>
        /// Async flush the associated session and end the unit of work.
        /// </summary>
        /// <returns></returns>
        Task CommitAsync();

        /// <summary>
        /// Reset current session.
        /// </summary>
        void ResetSession();

        /// <summary>
        /// Rollback changes for current transaction.
        /// </summary>
        void Rollback();

        /// <summary>
        /// Async rollback changes for current transaction.
        /// </summary>
        /// <returns></returns>
        Task RollbackAsync();
    }
}
