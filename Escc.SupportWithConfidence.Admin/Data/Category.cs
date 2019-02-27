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
        public int Sequence { get; set; }

        [Required(ErrorMessage ="Please enter the Flare code for the category")]
        public string Code { get; set; }

        [Required(ErrorMessage ="Please enter a description for the category")]
        public string Description { get; set; }

        public int? ParentId { get; set; }

        public int Depth { get; set; }

        public bool IsActive { get; set; }
    }
}