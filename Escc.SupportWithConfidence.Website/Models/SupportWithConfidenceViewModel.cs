using Escc.EastSussexGovUK.Mvc;
using Escc.SupportWithConfidence.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Escc.SupportWithConfidence.Website.Models
{
    public class SupportWithConfidenceViewModel : BaseViewModel
    {
        public Provider Provider { get; internal set; }
    }
}