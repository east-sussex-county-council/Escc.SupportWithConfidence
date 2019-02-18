using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Escc.EastSussexGovUK.Mvc;
using Escc.SupportWithConfidence.Admin.Models;
using Escc.SupportWithConfidence.Controls;
using Exceptionless;

namespace Escc.SupportWithConfidence.Admin.Controllers
{
    public class CategoriesController : Controller
    {
        public async Task<ActionResult> Index()
        {
            IProviderDataSource _dataSource = new SqlServerProviderDataSource();

            // Get categories from the database table Category
            var categories = await _dataSource.GetAllCategoriesWithProvider(false);

            // Get category collection that is structured as a family tree
            var categorymapper = new CategoryMapper(categories);

            var model = new CategoriesViewModel() { Categories = categorymapper.Categories };
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