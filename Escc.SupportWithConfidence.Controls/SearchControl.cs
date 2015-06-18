using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Escc.FormControls.WebForms.Validators;
using EsccWebTeam.SupportWithConfidence.Controls;

namespace Escc.SupportWithConfidence.Controls
{
    public class SearchControl : WebControl, INamingContainer
    {
        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            EnsureChildControls();

            ValidationSummary validationSummary = new EsccValidationSummary();
            validationSummary.DisplayMode = ValidationSummaryDisplayMode.BulletList;
            validationSummary.EnableClientScript = false;
            validationSummary.ShowSummary = true;

            Controls.Add(new LiteralControl("<div class=\"text\">"));

            var heading =
                new LiteralControl(
                    "<h1><img src=\"" + ResolveUrl("images/swc_logo_header.jpg") + "\" alt=\"'Support with confidence' approved care providers in East Sussex\" /></h1>");
            // LiteralControl heading = new LiteralControl("<hgroup><h1>East Sussex 'Support with confidence' directory</h1>");
            Controls.Add(heading);

            var subheading =
                new LiteralControl("<h2>Find providers of care and support services where you live</h2>");
            Controls.Add(subheading);
            Controls.Add(new LiteralControl("</div>")); // close .text

            var formBoxOpenCategory = new LiteralControl("<div class=\"formBox form simple-form\">");
            Controls.Add(formBoxOpenCategory);
            var categoryheading = new LiteralControl("<h2>List by category</h2>");
            Controls.Add(categoryheading);
            Controls.Add(new CategorySearchControl(new WebApiProviderDataSource()) {HasProvider = true});
            var formBoxCloseCategory = new LiteralControl("</div>");
            Controls.Add(formBoxCloseCategory);

            Controls.Add(new LiteralControl("<div class=\"text\">"));
            Controls.Add(validationSummary);
            Controls.Add(new LiteralControl("</div>")); // close .text

            var formBoxOpenPostcode = new LiteralControl("<div class=\"formBox form simple-form\">");
            Controls.Add(formBoxOpenPostcode);
            var postcodeheading = new LiteralControl("<h2>Or find all providers near you</h2>");
            Controls.Add(postcodeheading);
            Controls.Add(new PostcodeSearchControl
                {
                    ButtonText = "Search",
                    PostcodeText = "Postcode or town",
                    ShowOnResults = false
                });
            var formBoxClosePostcode = new LiteralControl("</div>");
            Controls.Add(formBoxClosePostcode);

            var formBoxOpenProvider = new LiteralControl("<div class=\"formBox form simple-form\">");
            Controls.Add(formBoxOpenProvider);
            var providerheading = new LiteralControl("<h2>Or find providers by name</h2>");
            Controls.Add(providerheading);
            Controls.Add(new ProviderSearchControl());
            var formBoxCloseProvider = new LiteralControl("</div>");
            Controls.Add(formBoxCloseProvider);

            var linkDisclaimer =
                new LiteralControl(
                    "<p class=\"text\"><a href=\"" + HttpUtility.HtmlAttributeEncode(ConfigurationManager.AppSettings["SupportWithConfidenceDisclaimerUrl"]) + "\">Disclaimer</a></p>");
            Controls.Add(linkDisclaimer);
        }
    }
}