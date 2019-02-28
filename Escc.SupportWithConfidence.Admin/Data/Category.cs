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

        public int Sequence { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Description { get; set; }

        public string Summary { get; set; }

        public int? ParentId { get; set; }

        public int Depth { get; set; }

        public bool IsActive { get; set; }
    }
}