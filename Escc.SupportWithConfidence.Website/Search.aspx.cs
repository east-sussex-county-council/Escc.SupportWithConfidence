using System;
using Escc.EastSussexGovUK.Skins;
using Escc.EastSussexGovUK.Views;
using Escc.EastSussexGovUK.WebForms;
using System.IO;
using Escc.Web;
using System.Globalization;
using System.Web.UI;

namespace Escc.SupportWithConfidence.Website
{
    public partial class Search : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Ensure there's one version of this URL so that the data is consistent in Google Analytics
            if (Path.GetFileName(Request.RawUrl).ToUpperInvariant().StartsWith("SEARCH.ASPX"))
            {
                new HttpStatus().MovedPermanently(ResolveUrl("~/"));
            }

            var skinnable = Master as BaseMasterPage;
            if (skinnable != null)
            {
                skinnable.Skin = new CustomerFocusSkin(ViewSelector.CurrentViewIs(MasterPageFile));
            }

            headContent.RssFeedUrl = new Uri(Request.Url, ResolveUrl("~/ProvidersRSS")).ToString();
        }


        /// <summary>
        /// Initializes the <see cref="T:System.Web.UI.HtmlTextWriter" /> object and calls on the child controls of the <see cref="T:System.Web.UI.Page" /> to render.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter" /> that receives the page content.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            // Get the HTML to be rendered 
            TextWriter tempWriter = new StringWriter(CultureInfo.CurrentCulture);
            base.Render(new HtmlTextWriter(tempWriter));
            string modifiedHtml = tempWriter.ToString();

            // Add a reload parameter to the form action, so that postbacks can be distinguished from the initial load in Google Analytics
            modifiedHtml = modifiedHtml.Replace(" action=\"./\"", " action=\"./?reload\"");

            // Send new HTML to be rendered instead
            writer.Write(modifiedHtml);
        }
    }
}