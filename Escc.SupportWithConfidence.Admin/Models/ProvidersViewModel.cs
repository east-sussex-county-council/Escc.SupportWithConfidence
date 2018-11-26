using Escc.EastSussexGovUK.Mvc;
using Escc.NavigationControls.WebForms;
using Escc.SupportWithConfidence.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Escc.SupportWithConfidence.Admin.Models
{
    public class ProvidersViewModel : BaseViewModel
    {
        public IList<Controls.Provider> Providers { get; internal set; }
        public PagingController PagingController { get; internal set; }
    }
}