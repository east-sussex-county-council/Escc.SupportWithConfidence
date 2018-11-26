using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Escc.SupportWithConfidence.Admin.Models;
using Escc.SupportWithConfidence.Controls;

namespace Escc.SupportWithConfidence.Admin.Controllers
{
    public class CategoriesController : Controller
    {
        public ActionResult Index()
        {
            IProviderDataSource _dataSource = new SqlServerProviderDataSource();

            // Get categories from the database table Category
            var categories = _dataSource.GetAllCategoriesWithProvider(false);

            // Get category collection that is structured as a family tree
            var categorymapper = new CategoryMapper(categories);

            return View(new CategoriesViewModel() { Categories = categorymapper.Categories });
        }
    }
}