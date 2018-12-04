using Escc.SupportWithConfidence.Controls;
using Escc.SupportWithConfidence.Website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Escc.SupportWithConfidence.Website.Controllers
{
    public class DetailController : Controller
    {
        // GET: Detail
        public ActionResult Index()
        {
            int reference;

            int.TryParse(Request.QueryString["ref"], result: out reference);

            var proMapper = new ProviderMapper(new WebApiProviderDataSource());
            proMapper.Map(reference);

            var model = new SupportWithConfidenceViewModel();
            if (proMapper.Providers.Count > 0)
            {
                model.Provider = proMapper.Providers[0];
               return View(model);
            }
            else return new HttpStatusCodeResult(410);
        }
    }
}