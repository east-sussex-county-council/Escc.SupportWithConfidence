using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Escc.SupportWithConfidence.Admin.Models;

namespace Escc.SupportWithConfidence.Admin.Controllers
{
    public class DefaultController : Controller
    {
        public ActionResult Index()
        {
            return View(new SupportWithConfidenceViewModel());
        }
    }
}