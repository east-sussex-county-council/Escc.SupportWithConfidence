using Escc.SupportWithConfidence.Website.Models;
using System;
using Escc.SupportWithConfidence.Controls;
using System.Linq;
using System.Web.Mvc;
using Exceptionless;
using Escc.EastSussexGovUK.Mvc;
using System.Threading.Tasks;
using System.Configuration;
using Escc.Net.Configuration;

namespace Escc.SupportWithConfidence.Website.Controllers
{
    public class ResultsController : Controller
    {
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var model = new SupportWithConfidenceViewModel();

            var controller = new SearchController(new WebApiProviderDataSource(new Uri(ConfigurationManager.AppSettings["SupportWithConfidenceApiBaseUrl"]), new ConfigurationWebApiCredentialsProvider()));
            model.Providers = await controller.GetResults();
            model.TotalResults = controller.TotalResults;
            model.QueryStringParameters = controller.QueryStringParameters;
            model.CategoryHeading = controller.CategoryHeading;

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
        public async Task<ActionResult> Index(string postcode, int? category=null)
        {
            var searcher = new LocationSearcher();
            var redirectTo = searcher.Search(postcode, category);
            if (redirectTo != null)
            {
                Response.Headers.Add("Location", redirectTo.ToString());
                return new HttpStatusCodeResult(303);
            }
            
            ModelState.AddModelError(String.Empty, $"No results were found for '{postcode}'. " +
                "If you entered a postcode please ensure this is a full postcode within East Sussex. If you entered a town please check the spelling.");

            var model = new SupportWithConfidenceViewModel();
            var controller = new SearchController(new WebApiProviderDataSource(new Uri(ConfigurationManager.AppSettings["SupportWithConfidenceApiBaseUrl"]), new ConfigurationWebApiCredentialsProvider()));
            model.Providers = await controller.GetResults();
            model.TotalResults = controller.TotalResults;
            model.QueryStringParameters = controller.QueryStringParameters;
            model.CategoryHeading = controller.CategoryHeading;

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