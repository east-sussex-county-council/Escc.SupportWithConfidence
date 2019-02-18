using Escc.SupportWithConfidence.Website.Models;
using Escc.SupportWithConfidence.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Escc.EastSussexGovUK.Mvc;
using Exceptionless;
using System.Threading.Tasks;
using Escc.Net.Configuration;
using System.Configuration;

namespace Escc.SupportWithConfidence.Website.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Handles the initial load of the search page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var model = new SupportWithConfidenceViewModel();

            // Get categories from the database table Category
            var dataSource = new WebApiProviderDataSource(new Uri(ConfigurationManager.AppSettings["SupportWithConfidenceApiBaseUrl"]), new ConfigurationWebApiCredentialsProvider());
            var categories = await dataSource.GetAllCategoriesWithProvider(true);

            // Get category collection that is structured as a family tree
            var categorymapper = new CategoryMapper(categories);

            model.Categories = categorymapper.Categories;

            var templateRequest = new EastSussexGovUKTemplateRequest(Request);
            try
            {
                model.WebChat = await templateRequest.RequestWebChatSettingsAsync();
            }
            catch (Exception ex)
            {
                // Catch and report exceptions - don't throw them and cause the page to fail
                ex.ToExceptionless().Submit();
            }
            try
            {
                model.TemplateHtml = await templateRequest.RequestTemplateHtmlAsync();
            }
            catch (Exception ex)
            {
                // Catch and report exceptions - don't throw them and cause the page to fail
                ex.ToExceptionless().Submit();
            }
            return View(model);
        }

        /// <summary>
        /// Handles a search for a postcode or town
        /// </summary>
        /// <param name="postcode">The postcode.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Index(string postcode)
        {
            var searcher = new LocationSearcher();
            var redirectTo = searcher.Search(postcode);
            if (redirectTo != null)
            {
                Response.Headers.Add("Location", redirectTo.ToString());
                return new HttpStatusCodeResult(303);
            }

            ModelState.AddModelError(String.Empty, $"No results were found for '{postcode}'. " +
                    "If you entered a postcode please ensure this is a full postcode within East Sussex. If you entered a town please check the spelling.");

            var model = new SupportWithConfidenceViewModel();

            // Get categories from the database table Category
            var dataSource = new WebApiProviderDataSource(new Uri(ConfigurationManager.AppSettings["SupportWithConfidenceApiBaseUrl"]), new ConfigurationWebApiCredentialsProvider());
            var categories = await dataSource.GetAllCategoriesWithProvider(true);

            // Get category collection that is structured as a family tree
            var categorymapper = new CategoryMapper(categories);

            model.Categories = categorymapper.Categories;

            var templateRequest = new EastSussexGovUKTemplateRequest(Request);
            try
            {
                model.WebChat = await templateRequest.RequestWebChatSettingsAsync();
            }
            catch (Exception ex)
            {
                // Catch and report exceptions - don't throw them and cause the page to fail
                ex.ToExceptionless().Submit();
            }
            try
            {
                model.TemplateHtml = await templateRequest.RequestTemplateHtmlAsync();
            }
            catch (Exception ex)
            {
                // Catch and report exceptions - don't throw them and cause the page to fail
                ex.ToExceptionless().Submit();
            }

            return View(model);
        }
        
    }
}