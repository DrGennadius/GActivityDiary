using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using GActivityDiary.Core.Models;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System.IO;

namespace GActivityDiary.Core.DataBase
{
    public class NHibernateHelper
    {
        private ISessionFactory _sessionFactory;

        private string _dataBaseFilePath = "ActivityDiary.db";

        public NHibernateHelper(string dataBaseFilePath)
        {
            Initialization(dataBaseFilePath);
        }

        public void Initialization(string dataBaseFilePath)
        {
            _dataBaseFilePath = dataBaseFilePath;
            Initialization();
        }

        public void Initialization()
        {
            bool dbExists = File.Exists(_dataBaseFilePath);
            _sessionFactory = Fluently.Configure()
                .Database(
                    SQLiteConfiguration.Standard
                    .UsingFile(_dataBaseFilePath)
                )
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Activity>())
                .ExposeConfiguration(cfg => new SchemaExport(cfg).Create(false, !dbExists))
                .BuildSessionFactory();
        }

        public ISession OpenSession()
        {
            if (_sessionFactory == null)
            {
                Initialization();
            }
            return _sessionFactory.OpenSession();
        }
    }
}
