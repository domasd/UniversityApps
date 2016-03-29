using System.Web.Http;

namespace IT
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{code}",
                defaults: new { code = RouteParameter.Optional}
            );

            config.Routes.MapHttpRoute("FlightSearch", "api/{controller}/{code}/{fromAirportCode}/{toAirportCode}");
            
        }
    }
}
