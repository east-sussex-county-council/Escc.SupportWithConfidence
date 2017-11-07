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
            return View(new SupportWithConfidenceViewModel());
        }
    }
}