using ChallengeMeLiServices.DataAccess.Daos;
using ChallengeMeLiServices.DataAccess.Daos.Interfaces;
using ChallengeMeLiServices.DataAccess.Repositories;
using ChallengeMeLiServices.DataAccess.Repositories.Interfaces;
using ChallengeMeLiServices.Services;
using ChallengeMeLiServices.Services.Interfaces;
using Unity;

namespace ChallengeMeLiServices.Web.Unity
{
    /// <summary>
    /// Configures unity mappings
    /// </summary>
    public static class UnityConfig
    {
        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        /// <returns>The unity container</returns>
        public static IUnityContainer GetConfiguredContainer()
        {
            var container = new UnityContainer();
            try
            {
                RegisterTypes(container);
            }
            catch
            {
                container.Dispose();
                throw;
            }
            return container;
        }

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        private static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IMutantService, MutantService>();
            container.RegisterType<IStatsService, StatsService>();

            container.RegisterType<IDnaDao, DnaDao>();
            container.RegisterType<IDnaRepository, DnaRepository>();
        }
    }
}