using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Escc.EastSussexGovUK.Mvc;

namespace Escc.SupportWithConfidence.Admin.Models
{
    public class AccreditationViewModel : BaseViewModel
    {
        public Accreditation Accreditation { get; set; }
        public IEnumerable<Accreditation> Accreditations { get; set; }
    }
}