using System;
using System.Collections.Generic;
using System.Text;
using Escc.AddressAndPersonalDetails;

namespace Escc.SupportWithConfidence.Controls
{
    public class Provider
    {
        public int Id { get; set; }

        public int FlareId { get; set; }

        public string ProviderName { get; set; }

        public string ContactName { get; set; }

        public BS7666Address Address { get; set; }

        public bool PublishAddress { get; set; }

        public string Telephone { get; set; }

        public string Mobile { get; set; }

        public string Website { get; set; }

        public string Fax { get; set; }
        public string Email { get; set; }

        public int Easting { get; set; }

        public int Northing { get; set; }


        public bool PublishToWeb { get; set; }

        // Need to include the image later


        public string Experience { get; set; }

        public string Background { get; set; }

        public string Expertise { get; set; }

        public IList<Accreditation> Accreditations { get; set; } = new List<Accreditation>();

        public string Availability { get; set; }


      
        public string Costs { get; set; }

        public string Coverage { get; set; }
        public string Coverage2 { get; set; }

        

        public string CrbCheckDate { get; set; }
        public string CqcCheckDate { get; set; }
        public string Crb { get; set; }

        public bool BwcMember { get; set; }

        public bool IsDeleted { get; set; }

        public string LastModified { get; set; }

        /// <summary>
        /// Categories where this provider should be listed
        /// </summary>
        public IList<Category> Categories { get; internal set; } = new List<Category>();

        /// <summary>
        /// IDs of categories where this provider should be listed - used when editing the provider
        /// </summary>
        public string[] CategoryIds { get; set; }

        public string Services { get; set; }

        public int TotalResults { get; set; }

        public int PhotographId { get; set; }

        public bool IsPaTrained { get; set; }
        public Uri ImageUrl { get; set; }
    }
}