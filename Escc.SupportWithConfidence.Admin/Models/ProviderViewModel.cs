using Escc.EastSussexGovUK.Mvc;
using Escc.NavigationControls.WebForms;
using Escc.SupportWithConfidence.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Escc.SupportWithConfidence.Admin.Models
{
    public class ProviderViewModel : BaseViewModel
    {
        public Provider Provider { get; set; }

        public IEnumerable<Accreditation> Accreditations { get;set;}

        public bool RemovePhoto { get; set; }
    }
}