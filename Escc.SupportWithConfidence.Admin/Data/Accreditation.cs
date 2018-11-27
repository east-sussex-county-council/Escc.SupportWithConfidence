using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Escc.SupportWithConfidence.Admin.Data
{
    public class Accreditation
    {
        [Key]
        public int AccreditationId { get; set; }

        [Required(ErrorMessage ="Please enter a name for the accreditation")]
        public string Name { get; set; }

        [Url(ErrorMessage ="Please enter a complete website address starting with http:// or https://")]
        public string Website { get; set; }
    }
}