using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Escc.SupportWithConfidence.Admin
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
                defaults: new { controller = "Default", action = "Index" }
            );

            // Home page
            routes.MapRoute(
                name: "Default",
                url: String.Empty,
                defaults: new { controller = "Default", action = "Index" }
            );
        }
    }
}
