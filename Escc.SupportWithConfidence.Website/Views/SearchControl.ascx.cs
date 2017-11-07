using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Escc.SupportWithConfidence.Website.Views
{
    public partial class SearchControl : ViewUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

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