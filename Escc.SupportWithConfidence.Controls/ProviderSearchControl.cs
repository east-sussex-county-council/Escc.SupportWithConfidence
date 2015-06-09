using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Escc.SupportWithConfidence.Controls
{
    public class ProviderSearchControl : WebControl, INamingContainer
    {
        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            EnsureChildControls();

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

            var btnSearch = new Button {ID = "btnSearch", Text = @"Search", CssClass = "button"};
            btnSearch.Click += btnSearch_Click;

            Controls.Add(lblProvider);
            Controls.Add(txbProvider);
            Controls.Add(btnSearch);
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