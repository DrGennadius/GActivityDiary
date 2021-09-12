using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GActivityDiary.Core.DataBase
{
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
