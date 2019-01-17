using System;
using System.Collections.Generic;
using System.Text;
using Escc.AddressAndPersonalDetails;

namespace Escc.SupportWithConfidence.Controls
{
    public class Provider : IResult
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

        public string CategoryList { get; set; }

        public string Services { get; set; }

        public int TotalResults { get; set; }

        public int PhotographId { get; set; }

        public bool IsPaTrained { get; set; }
        public Uri ImageUrl { get; set; }

        #region IResult Members

        public string View()
        {
            var html = new StringBuilder();
            html.Append("<dl class=\"itemDetail\"><dt>Name</dt>");
            html.Append(String.Format("<dd><p><a href=\"provider.aspx?ref={0}\">{1}</a></p></dd>", FlareId, ProviderName));
            if (PublishAddress)
            {
                html.Append(String.Format("<dt>Address</dt><dd>{0}</dd>", Address.GetSimpleAddress()));
            }

            if (!string.IsNullOrEmpty(Telephone))
            {
                html.Append(String.Format("<dt>Telephone</dt><dd>{0}</dd>", Telephone));
            }
            if (!string.IsNullOrEmpty(Email))
            {
                html.Append(String.Format("<dt>Email</dt><dd>{0}</dd>", Email));
            }
            html.Append("</dl>");



            return html.ToString();
        }

        #endregion
    }
}