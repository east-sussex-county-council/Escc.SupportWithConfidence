﻿using System.Collections.Generic;
using Escc.EastSussexGovUK.Mvc;
using Escc.SupportWithConfidence.Admin.Data;

namespace Escc.SupportWithConfidence.Admin.Models
{
    public class CategoryViewModel : BaseViewModel
    {
        public Category Category { get; set; }

        public IEnumerable<Category> PossibleParentCategories { get; set; }
    }
}