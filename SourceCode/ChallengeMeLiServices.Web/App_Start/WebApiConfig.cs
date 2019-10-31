using System.Web.Http;
using ChallengeMeLiServices.Web.Unity;

namespace ChallengeMeLiServices.Web
{
    public static class WebApiConfig
    {
        /// <summary>
        /// API's root uri
        /// </summary>
        public const string RootApiUri = "api/challenge-meli";

        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            UnityResolver resolver = new UnityResolver(UnityConfig.GetConfiguredContainer());
            config.DependencyResolver = resolver;

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
