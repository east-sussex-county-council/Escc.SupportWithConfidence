using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Escc.SupportWithConfidence.Admin.Models;
using Escc.SupportWithConfidence.Controls;

namespace Escc.SupportWithConfidence.Admin.Controllers
{
    public class EditCategoryController : Controller
    {
        public ActionResult Index(int cat)
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

            return View(model);
        }
    }
}