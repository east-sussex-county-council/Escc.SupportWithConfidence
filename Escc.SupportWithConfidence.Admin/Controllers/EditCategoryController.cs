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
    public class EditCategoryController : Controller
    {
        public async Task<ActionResult> Index(int cat)
        {
            var model = new EditCategoryViewModel();

            IProviderDataRepository dataSource = new SqlServerProviderDataRepository();
            var categories = new CategoryMapper(dataSource.GetCategoryById(cat)).Categories;

            var count = 0;
            foreach (var parentCategory in categories)
            {
                foreach (var childCategory in parentCategory.Categories)
                {
                    if (parentCategory.Id != cat && childCategory.Id != cat) continue;
                    while (count < 1)
                    {
                        model.ParentCategory = parentCategory;
                        model.CategoryToEdit = childCategory;
                        count++;
                    }
                }
            }

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