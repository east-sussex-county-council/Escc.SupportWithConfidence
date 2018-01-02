using Escc.SupportWithConfidence.Website.Models;
using Escc.Web;
using System;
using Escc.SupportWithConfidence.Controls;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Escc.SupportWithConfidence.Website.Controllers
{
    public class ResultsController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View(new SupportWithConfidenceViewModel());
        }

        /// <summary>
        /// Handles a search for a postcode or town
        /// </summary>
        /// <param name="postcode">The postcode.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(string postcode, int? category=null)
        {
            var searcher = new LocationSearcher();
            var redirectTo = searcher.Search(postcode, category);
            if (redirectTo != null)
            {
                new HttpStatus().SeeOther(redirectTo);
            }
            else
            {
                ModelState.AddModelError(String.Empty, $"No results were found for '{postcode}'. " +
                    "If you entered a postcode please ensure this is a full postcode within East Sussex. If you entered a town please check the spelling.");
            }

            return View(new SupportWithConfidenceViewModel());
        }
    }
}