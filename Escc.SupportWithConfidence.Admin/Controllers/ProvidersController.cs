using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Escc.NavigationControls.WebForms;
using Escc.SupportWithConfidence.Admin.Models;
using Escc.SupportWithConfidence.Controls;

namespace Escc.SupportWithConfidence.Admin.Controllers
{
    public class ProvidersController : Controller
    {
        public ActionResult Index()
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
            
            return View(model);
        }
    }
}