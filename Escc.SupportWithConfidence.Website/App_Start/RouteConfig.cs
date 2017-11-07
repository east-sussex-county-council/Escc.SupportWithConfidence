using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Escc.SupportWithConfidence.Website
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Preserve old URLs from when this was a WebForms app
            routes.MapRoute(
                name: "WebForms",
                url: "{controller}.aspx",
                defaults: new { controller = "Home", action = "Index" }
            );

            // Home page
            routes.MapRoute(
                name: "Default",
                url: String.Empty,
                defaults: new { controller = "Home", action = "Index" }
            );
        
            // RSS feed
            routes.MapRoute(
                name: "ProvidersRSS",
                url: "ProvidersRSS/{action}/{id}",
                defaults: new { controller = "ProvidersRSS", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
