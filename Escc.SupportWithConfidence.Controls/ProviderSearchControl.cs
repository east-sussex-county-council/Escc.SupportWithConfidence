using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EsccWebTeam.EastSussexGovUK;

namespace Escc.SupportWithConfidence.Controls
{
    public class ProviderSearchControl : WebControl, INamingContainer
    {
        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            EnsureChildControls();


            var formPartOpen = new LiteralControl("<div class=\"formPart\">");
            var lblProvider = new Label
                {
                    Text = @"Provider name",
                    ID = "lblProvider",
                    AssociatedControlID = "txbProvider",
                    CssClass = "formLabel"
                };
            var txbProvider = new TextBox
                {
                    ID = "txbProvider",
                    CssClass = "formControl",
                    TextMode = TextBoxMode.SingleLine
                };
            var formPartClose = new LiteralControl("</div>");

            var formButtonsOpen = new LiteralControl("<div class=\"formButtons\">");
            var btnSearch = new Button {ID = "btnSearch", Text = @"Search", CssClass = "button"};
            var formButtonsClose = new LiteralControl("</div>");
            


            btnSearch.Click += btnSearch_Click;

            var siteContext = new EastSussexGovUKContext();

            if (siteContext.ViewIsLegacy)
            {
                Controls.Add(formPartOpen);
                Controls.Add(lblProvider);
                Controls.Add(txbProvider);
                Controls.Add(formPartClose);
                Controls.Add(formButtonsOpen);
                Controls.Add(btnSearch);
                Controls.Add(formButtonsClose);
            }
            else
            {
                Controls.Add(lblProvider);
                Controls.Add(txbProvider);
                Controls.Add(btnSearch);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var txbProvider = (TextBox) FindControl("txbProvider");


            HttpContext.Current.Response.Redirect(txbProvider.Text.Length > 0
                                                      ? String.Format("results.aspx?s={0}", txbProvider.Text)
                                                      : "results.aspx");
        }
    }
}