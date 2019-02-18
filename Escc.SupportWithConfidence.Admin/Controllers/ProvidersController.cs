using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Escc.EastSussexGovUK.Mvc;
using Escc.NavigationControls.WebForms;
using Escc.SupportWithConfidence.Admin.Models;
using Escc.SupportWithConfidence.Controls;
using Exceptionless;

namespace Escc.SupportWithConfidence.Admin.Controllers
{
    public class ProvidersController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var model = new ProvidersViewModel();
            model.PagingController = new PagingController();

            var pageIndex = 1;
            const int pageSize = 10;
            var mapper = new ProviderMapper(new SqlServerProviderDataRepository());

            if (System.Web.HttpContext.Current.Request.QueryString["page"] != null)
            {
                pageIndex = System.Convert.ToInt16(System.Web.HttpContext.Current.Request.QueryString["page"]);
                mapper.Map(pageIndex, pageSize);
            }
            else
            {
                mapper.Map(pageIndex, pageSize);
            }

            model.Providers = mapper.Providers;
            model.PagingController.TotalResults = mapper.TotalResults;
            model.PagingController.ResultsTextSingular = "provider";
            model.PagingController.ResultsTextPlural = "providers";
            model.PagingController.PageSize = pageSize;

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