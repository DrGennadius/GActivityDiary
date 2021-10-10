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
        ISession Session { get; }

        ITransaction BeginTransaction();

        void Commit();

        Task CommitAsync();

        void ResetSession();

        void Rollback();

        Task RollbackAsync();
    }
}
