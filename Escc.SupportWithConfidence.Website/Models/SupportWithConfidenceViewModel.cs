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

        public IList<IResult> Providers { get; internal set; }
        public QueryParameter QueryStringParameters { get; set; }

        public IList<Category> Categories { get; internal set; }
        public int TotalResults { get; internal set; }
        public string CategoryHeading { get; internal set; }
        public IHtmlString CategorySummary { get; internal set; }
    }
}