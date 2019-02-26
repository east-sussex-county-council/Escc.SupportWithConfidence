using Escc.EastSussexGovUK.Mvc;
using Escc.Net;
using Escc.Net.Configuration;
using Escc.SupportWithConfidence.Controls;
using Escc.SupportWithConfidence.Website.Models;
using Exceptionless;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Escc.SupportWithConfidence.Website.Controllers
{
    public class DetailController : Controller
    {
        // GET: Detail
        public async Task<ActionResult> Index()
        {
            var model = new SupportWithConfidenceViewModel();

            int reference;

            int.TryParse(Request.QueryString["ref"], result: out reference);

            var proMapper = new ProviderMapper(new WebApiProviderDataSource(new Uri(ConfigurationManager.AppSettings["SupportWithConfidenceApiBaseUrl"]), new HttpClientProvider(null, new ConfigurationWebApiCredentialsProvider())));
            await proMapper.Map(reference);

            if (proMapper.Providers.Count > 0)
            {
                model.Provider = proMapper.Providers[0];

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
            else return new HttpStatusCodeResult(410);
        }
    }
}