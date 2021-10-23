using NHibernate;

namespace GActivityDiary.Core.NHibernate
{
    /// <summary>
    /// NHibernate session.
    /// </summary>
    public class NHibernateSession
    {
        private readonly NHibernateFactoryProxy _nHibernateFactoryProxy;

        public NHibernateSession(NHibernateFactoryProxy nHibernateFactoryProxy)
        {
            _nHibernateFactoryProxy = nHibernateFactoryProxy;
        }

        public ISession OpenSession()
        {
            var session = _nHibernateFactoryProxy.SessionFactory.OpenSession();

            //new SchemaExport(_nHibernateFactoryProxy.Configuration)
            //    .Execute(true, true, false, session.Connection, null);

            return session;
        }
    }
}
