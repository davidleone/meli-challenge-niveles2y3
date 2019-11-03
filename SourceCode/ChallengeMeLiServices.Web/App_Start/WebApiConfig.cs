using System.Web.Http;
using ChallengeMeLiServices.Web.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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

            //I change the json serialization in order to match the required in the challenge (json snake_case)
            JsonSerializerSettings serializer = config.Formatters.JsonFormatter.SerializerSettings;
            serializer.ContractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };

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
