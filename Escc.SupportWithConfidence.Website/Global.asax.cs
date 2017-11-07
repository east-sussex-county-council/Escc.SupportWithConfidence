using Escc.Web;
using System.IO;
using System.Web.Mvc;
using System.Web.Routing;

namespace Escc.SupportWithConfidence.Website
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_BeginRequest()
        {
            // Redirect old WebForms URL so that data for the home page is consistent in Google Analytics
            if (Path.GetFileName(Request.RawUrl).ToUpperInvariant().StartsWith("SEARCH.ASPX"))
            {
                new HttpStatus().MovedPermanently(System.Web.VirtualPathUtility.ToAbsolute("~/"));
            }
        }

    }
}
