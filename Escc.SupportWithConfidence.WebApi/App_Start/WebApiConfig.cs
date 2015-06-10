using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Escc.SupportWithConfidence.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "ImageApi",
                routeTemplate: "api/{controller}/{action}/{id}"
            );
            config.Routes.MapHttpRoute(
               name: "ProvidersApi",
               routeTemplate: "api/{controller}/{id}"
           );
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}",
                defaults: new { action = "GetAll" }
            );

        }
    }
}
