using ChallengeMeLiServices.DataAccess.Maps;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;

namespace ChallengeMeLiServices.DataAccess
{
    /// <summary>
    /// Static class to hold the Database Connection.
    /// </summary>
    public class SessionManager
    {
        /// <summary>
        /// NHibernate Session Factory
        /// </summary>
        private static ISessionFactory _sessionFactory;

        /// <summary>
        /// Get an Opened Session. CloseSession() method must be called after this one.
        /// </summary>
        /// <returns>NHibernate opened ISession</returns>
        public static ISession GetSession()
        {
            if (_sessionFactory == null)
                _sessionFactory = FluentConfiguration();

            return _sessionFactory.OpenSession();
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
                    .Host("aaywwm7tlbw572.cno6zl5xdzuy.sa-east-1.rds.amazonaws.com")
                    .Username("postgres")
                    .Password("Postgres2019!")
                    .Database("myDatabase")
                    .Port(5432)
                    ).Dialect<NHibernate.Dialect.PostgreSQL82Dialect>())
                .Mappings(x => x.FluentMappings.AddFromAssemblyOf<DnaMap>())
                .BuildSessionFactory();
        }

        /// <summary>
        /// Method specific for Unit Tests purposes. Don't use it!
        /// </summary>
        /// <param name="sessionFactory">Session Factory</param>
        public static void SetMockedSessionForTests(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }
    }
}
