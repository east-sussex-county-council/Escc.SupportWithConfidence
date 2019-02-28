﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Escc.SupportWithConfidence.Admin.Data
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Display(Name ="Sort order (if not alphabetical)")]
        public int Sequence { get; set; }

        [Required(ErrorMessage ="Please enter the Flare code for the category")]
        [Display(Name ="Flare category code")]
        public string Code { get; set; }

        [Required(ErrorMessage ="Please enter a name for the category")]
        [Display(Name ="Name")]
        public string Description { get; set; }

        [Display(Name ="Parent category")]
        public int? ParentId { get; set; }

        public int Depth { get; set; }

        [Display(Name="Publish on website")]
        public bool IsActive { get; set; }
    }
}