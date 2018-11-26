using Escc.EastSussexGovUK.Mvc;
using Escc.SupportWithConfidence.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Escc.SupportWithConfidence.Admin.Models
{
    public class EditCategoryViewModel : BaseViewModel
    {
        public Category CategoryToEdit { get; set; }
        public Category ParentCategory { get; internal set; }
    }
}