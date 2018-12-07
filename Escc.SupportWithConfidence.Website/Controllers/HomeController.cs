using Escc.SupportWithConfidence.Website.Models;
using Escc.SupportWithConfidence.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Escc.SupportWithConfidence.Website.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Handles the initial load of the search page
        /// </summary>
        /// <returns></returns>
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
        public ActionResult Index(string postcode)
        {
            var searcher = new LocationSearcher();
            var redirectTo = searcher.Search(postcode);
            if (redirectTo != null)
            {
                Response.Headers.Add("Location", redirectTo.ToString());
                return new HttpStatusCodeResult(303);
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