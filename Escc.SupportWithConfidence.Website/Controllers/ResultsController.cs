using Escc.SupportWithConfidence.Website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Escc.SupportWithConfidence.Website.Controllers
{
    public class ResultsController : Controller
    {
        // GET: Results
        public ActionResult Index()
        {
            return View(new SupportWithConfidenceViewModel());
        }
    }
}