using ChallengeMeLiServices.DataAccess.Maps;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;

namespace ChallengeMeLiServices.DataAccess
{
    /// <summary>
    /// Static class to hold the Database Connection.
    /// </summary>
    public static class SessionManager
    {
        private static ISessionFactory _sessionFactory;
        private static ISession _session;

        /// <summary>
        /// Private method to initialize the FluentNHibernate configuration against the database.
        /// </summary>
        private static void Configure() {
            if (_sessionFactory == null)
            {
                _sessionFactory = FluentConfiguration();
            }
        }

        /// <summary>
        /// Get an Opened Session. CloseSession() method must be called after this one.
        /// </summary>
        /// <returns>NHibernate opened ISession</returns>
        public static ISession GetSession()
        {
            Configure();

            if (_session == null || (_session != null && _session.IsOpen))
            {
                _session = _sessionFactory.OpenSession();
            }

            return _session;
        }

        /// <summary>
        /// Close the current opened session.
        /// </summary>
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
        /// Private method to configure the string connection to database and the mappings by FluentNHibernate.
        /// </summary>
        /// <returns></returns>
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
        }
    }
}
