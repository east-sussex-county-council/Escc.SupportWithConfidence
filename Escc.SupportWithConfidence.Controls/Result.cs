using System;
using System.Text;

namespace Escc.SupportWithConfidence.Controls
{
    public class Result : IResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool PublishAddress { get; set; }
        public string BasedIn { get; set; }
        public string Telephone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public int Distance { get; set; }
        public string Coverage { get; set; }

        public int CurrentPage { get; set; }
        public int TotalResults { get; set; }
        public string QueryString { get; set; }
        public bool ShowDistance { get; set; }
        public string CategoryList { get; set; }

        public string View()
        {
            var html = new StringBuilder();
            html.Append("<div class=\"eventSummary\" ><dl class=\"itemDetail vevent\"><dt>Name</dt>");
            html.Append(String.Format("<dd class=\"summary\"><p><strong><a href=\"detail.aspx?{0}&amp;ref={1}\">{2}</a></strong></p></dd>", System.Web.HttpUtility.HtmlEncode(QueryString), Id, Name));
            if (PublishAddress)
            {
                html.Append(String.Format("<dt>Address</dt><dd>{0}</dd>", Address));
            }

            if (Telephone.Length > 0)
            {
                html.Append(String.Format("<dt>Telephone</dt><dd class=\"telephone\">{0}</dd>", Telephone));
            }

            if (Mobile.Length > 0)
            {
                html.Append(String.Format("<dt>Mobile</dt><dd class=\"mobile\">{0}</dd>", Mobile));
            }

            html.Append(String.Format("<dt>Based in</dt><dd class=\"location\">{0}</dd>", BasedIn));



            if (ShowDistance)
            {
                var dis = Distance > 1 ? "miles (approximately)" : "mile (approximately)";

                html.Append(String.Format("<dt>Distance from me</dt><dd>{0} {1}</dd>", Distance, dis));
            }

            if (Coverage.Trim().Length > 0)
            {
                html.Append(String.Format("<dt>Coverage</dt><dd class=\"coverage\">{0}</dd>", Coverage));
            }


            html.Append(String.Format("<dt>Category listings</dt><dd class=\"category\"><ul>{0}</ul></dd></dl></div>", CategoryList));


            return html.ToString();
        }


    }
}
