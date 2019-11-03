using ChallengeMeLiServices.DataAccess.Maps;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;

namespace ChallengeMeLiServices.DataAccess
{
    public static class SessionManager
    {
        private static ISessionFactory _sessionFactory;
        private static ISession _session;

        private static void Configure() {
            if (_sessionFactory == null)
            {
                _sessionFactory = FluentConfiguration();
            }
        }

        public static ISession GetSession()
        {
            Configure();

            if (_session == null || (_session != null && _session.IsOpen))
            {
                _session = _sessionFactory.OpenSession();
            }

            return _session;
        }

        public static void CloseSession()
        {
            if (_session != null && _session.IsOpen)
            {
                _session.Close();
                _session.Dispose();
                _session = null;
            }
        }

        /// <summary>
        /// Método que crea la session factory
        /// </summary>
        private static ISessionFactory FluentConfiguration()
        {
            return Fluently
                .Configure()
                .Database(PostgreSQLConfiguration.PostgreSQL82.ConnectionString(x => x
                    .Host("database-1.cno6zl5xdzuy.sa-east-1.rds.amazonaws.com")
                    .Username("postgres")
                    .Password("Postgres2019!")
                    .Database("myDatabase")
                    .Port(5432)
                    ).Dialect<NHibernate.Dialect.PostgreSQL82Dialect>())
                .Mappings(x => x.FluentMappings.AddFromAssemblyOf<DnaMap>())
                .BuildSessionFactory();
            /*return Fluently
                .Configure()
                .Database(PostgreSQLConfiguration.PostgreSQL82.ConnectionString(x => x
                    .Host("localhost")
                    .Username("postgres")
                    .Password("postgres")
                    .Database("postgres")
                    .Port(5432)
                    ).Dialect<NHibernate.Dialect.PostgreSQL82Dialect>())
                .Mappings(x => x.FluentMappings.AddFromAssemblyOf<DnaMap>())
                .BuildSessionFactory();*/
        }
    }
}
