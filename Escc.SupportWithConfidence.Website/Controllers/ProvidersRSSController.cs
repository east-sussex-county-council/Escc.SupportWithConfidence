using Escc.SupportWithConfidence.Controls;
using System;
using System.Web.Mvc;

namespace Escc.SupportWithConfidence.Website.Controllers
{
    public class ProvidersRSSController : Controller
    {
        // create a new search controller to get providers
        public SearchController controller = new SearchController(new WebApiProviderDataSource());

        // GET: ProvidersRSS
        public ActionResult Index()
        {
            // set the page size parameter to the max int16 value, this allows us to retrieve all providers at once
            controller.QueryStringParameters.PageSize = Int16.MaxValue;
            // Get a list of all providers using the controllers GetResults() Method
            var providersList = controller.GetResults();
            // return the list to the view
            return View(providersList);
        }

    } 
}