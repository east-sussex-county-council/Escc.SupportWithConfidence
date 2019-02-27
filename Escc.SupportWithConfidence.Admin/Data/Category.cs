using System;
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

        [Required(ErrorMessage ="Please enter a number reflecting the order the category should appear in")]
        [Display(Name ="Order")]
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