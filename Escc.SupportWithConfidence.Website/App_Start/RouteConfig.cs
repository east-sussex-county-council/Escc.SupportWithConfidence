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

            // Using map page route here as a way to use an .aspx file as a view with mvc
            routes.MapPageRoute(
                "Default",
                "Home/Index",
                "~/"
                );

            routes.MapRoute(
                name: "ProvidersRSS",
                url: "ProvidersRSS/{action}/{id}",
                defaults: new { controller = "ProvidersRSS", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
