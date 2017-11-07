using System;
using System.Configuration;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Escc.DatabaseFileControls.WebForms;
using Escc.FormControls.WebForms.Validators;

namespace Escc.SupportWithConfidence.Controls
{
    public class ProviderDetailControl : WebControl, INamingContainer
    {

        public ProviderDetailControl()
            : base(HtmlTextWriterTag.Div)
        { }

        #region Override Methods

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            EnsureChildControls();


            int reference;

            int.TryParse(HttpContext.Current.Request.QueryString["ref"], result: out reference);



            var proMapper = new ProviderMapper(new WebApiProviderDataSource());
            proMapper.Map(reference);

            if (proMapper.Providers.Count > 0)
            {
                Provider p = proMapper.Providers[0];


                // DataTable dt = GetData();
                var html = new StringBuilder();
                html.Append("<h1>" + p.ProviderName + "</h1>");

                Uri imageUrl = MultiFileAttachmentBaseControl.GetImageUrl(p.PhotographId, ImageUploader.DotNetProjectName);
                if (imageUrl != null)
                {

                    var img = new HtmlImage {Src = imageUrl.AbsoluteUri, Alt = p.ProviderName};
                    img.Attributes["class"] = "photo";
                    Controls.Add(img);



                }

                html.Append("<h2>Contact details</h2>");


                html.Append("<div class=\"vcard\">");
                if (p.PublishAddress)
                {

                    html.Append("<p>");

                    if (p.ContactName.Length > 0)
                    {
                        html.Append("<span class=\"n\">" + p.ContactName + "</span><br />");
                    }
                    else
                    {
                        html.Append("<span class=\"org\">" + p.ProviderName + "</span><br />");
                    }
                    html.Append("<span class=\"location adr\">");
                    if (p.Address.Paon.Length > 4)
                    {
                        html.Append("<span class=\"street-address\">" + p.Address.Paon + " " + p.Address.Saon + "<br />" + p.Address.StreetName + "</span><br />");
                    }
                    else
                    {
                        html.Append("<span class=\"street-address\">" + p.Address.Paon + " " + p.Address.Saon + " " + p.Address.StreetName + "</span><br />");
                    }

                    html.Append("<span class=\"locality\">" + p.Address.Town + "</span><br />");
                    if (p.Address.Locality.Length > 0)
                    {
                        html.Append("<span class=\"region\">" + p.Address.Locality + " " + "</span><br />");
                    }
                    html.Append("<span class=\"postal-code\">" + p.Address.Postcode + "</span>");
                    html.Append("</span>");
                    html.Append("</p>");
                }
                else
                {

                    html.Append("<p>");

                    if (p.ContactName.Length > 0)
                    {
                        html.Append("<span class=\"n\">" + p.ContactName + "</span><br />");
                    }
                    else
                    {
                        html.Append("<span class=\"org\">" + p.ProviderName + "</span><br />");
                    }
                    html.Append("</p>");
                }
                html.Append("<p>");
                if (p.Telephone.Trim().Length > 0)
                {
                    html.Append("<span class=\"tel\">");
                    html.Append("<span class=\"aural\">");
                    html.Append("<span class=\"type\"></span> ");
                    html.Append("</span>");

                    html.Append("Phone: <span class=\"value\">" + p.Telephone + "</span>");

                    html.Append("</span>");
                }
                if (p.Mobile.Trim().Length > 0)
                {
                    html.Append("<br />");
                    html.Append("<span class=\"tel\">");
                    html.Append("<span class=\"aural\">");
                    html.Append("<span class=\"type\"></span> ");
                    html.Append("</span>");
                    html.Append("Mobile: <span class=\"value\">" + p.Mobile + "</span>");
                    html.Append("</span>");
                }

                if (p.Email.Trim().Length > 0)
                {
                    html.Append("<br />");
                    html.Append("Email: <a class=\"email\" href=\"mailto:" + p.Email + "\">" + p.Email + "</a><br />");
                }

                if (p.Website.Trim().Length > 0)
                {
                    html.Append("Website: <a class=\"url\" href=\"" + p.Website + "\">" + p.Website + "</a>");
                }
                html.Append("</p>");
                html.Append("</div>");

                if (p.Services.Trim().Length > 0)
                {
                    html.Append("<h2>List of services</h2>");
                    html.Append("<p>" + p.Services + "</p>");
                }


                if (p.Coverage.Trim().Length > 0 & p.Coverage2.Trim().Length > 0)
                {
                    html.Append("<h2>Coverage</h2>");
                    html.Append("<p>" + p.Coverage + " " + p.Coverage2 + "</p>");
                }
                else
                {
                    if (p.Coverage.Trim().Length > 0)
                    {
                        html.Append("<h2>Coverage</h2>");
                        html.Append("<p>" + p.Coverage + "</p>");
                    }
                }










                if (p.Availability.Trim().Length > 0)
                {
                    html.Append("<h2>Availability</h2>");
                    html.Append("<p>" + p.Availability + "</p>");
                }

                if (p.Costs.Length > 0)
                {
                    html.Append("<h2>Charging and related information</h2>");
                    html.Append("<p>" + p.Costs + "</p>");
                }


                if (p.Expertise.Trim().Length > 0)
                {
                    html.Append("<h2>Expertise</h2>");
                    html.Append("<p>" + p.Expertise + "</p>");
                }

                if (p.Experience.Trim().Length > 0)
                {
                    html.Append("<h2>Experience</h2>");
                    html.Append("<p>" + p.Experience + "</p>");
                }

                if (p.Background.Trim().Length > 0)
                {
                    html.Append("<h2>Background</h2>");
                    html.Append("<p>" + p.Background + "</p>");
                }


                if (p.Accreditation.Trim().Length > 0)
                {
                    html.Append("<h2>Accreditation</h2>");
                    html.Append("<p>" + p.Accreditation + "</p>");
                }


                html.Append(String.Format("<h2>Category listings</h2><ul>{0}</ul>", p.CategoryList));


                if (p.Crb.Length > 0)
                {
                    html.Append("<h2>Criminal records information:</h2>");
                    html.Append("<p>" + p.Crb + "</p>");
                }
                else
                {

                    if (p.CrbCheckDate.Trim().Length > 0)
                    {
                        html.Append("<h2>Date of last Criminal Records check</h2>");
                        html.Append("<p>" + p.CrbCheckDate + "</p>");
                    }
                    else if (p.CqcCheckDate.Trim().Length > 0)
                    {
                        html.Append("<h2>Date of Care Quality Commission (CQC) Registration</h2>");
                        html.Append("<p>" + p.CqcCheckDate + "</p>");
                    }
                }





                if (p.BwcMember)
                {
                    html.Append("<img src=\"/socialcare/athome/approvedproviders/images/bwc_logo.jpg\" alt=\"Buy with Confidence Member\" class=\"bwclogo\">");
                }


                Controls.Add(new LiteralControl(html.ToString()));

                var linkBottomSearch = new LiteralControl("<p><a href=\"" + VirtualPathUtility.ToAbsolute("~/") + "\" class=\"newsearch\">New search</a></p>");
                Controls.Add(linkBottomSearch);

                var linkDisclaimer = new LiteralControl("<p><a href=\"" + HttpUtility.HtmlAttributeEncode(ConfigurationManager.AppSettings["SupportWithConfidenceDisclaimerUrl"]) + "\">Disclaimer</a></p>");
                Controls.Add(linkDisclaimer);

                /*
                 <div class="vcard">
                    <p>
                        <span class="org">Hutson limited</span><br>
                        <span class="location adr">
                           Address:<span class="street-address">32 Rowan Avenue</span><br>
                            <span class="locality">Hove</span><br>
                            <span class="region">East Sussex</span> 
                            <span class="postal-code">BN3 7JG</span>
                        </span>
                    </p>
              
                    <p>
                        <span class="tel">
                            <span class="aural">
                                <span class="type">Work</span> 
                            </span>
                            Phone: <span class="value">07918 902721</span>
                        </span>
                        <br>
                         <span class="tel">
                            <span class="aural">
                                <span class="type">Cell</span> 
                            </span>
                            Mobile: <span class="value">07918 902721</span>
                        </span>
                        <br>
                        Email: <a class="email" href="mailto:info@brightonscenic.co.uk">info@brightonscenic.co.uk</a><br>
                        Website: <a class="url" href="http://www.brightonscenic.co.uk/">www.brightonscenic.co.uk</a>
                    </p>
                </div>
                 */
            }
            else
            {
                var noResultsValidator = new CustomValidator
                    {
                        Display = ValidatorDisplay.None,
                        EnableClientScript = false,
                        IsValid = false,
                        ErrorMessage =
                            @"Unfortunately no details for the provider could be found. Please try searching again with different criteria."
                    };
                ValidationSummary validationSummary = new EsccValidationSummary();
                validationSummary.DisplayMode = ValidationSummaryDisplayMode.BulletList;
                validationSummary.EnableClientScript = false;
                validationSummary.ShowSummary = true;

                Controls.Add(validationSummary);
                validationSummary.Controls.Add(noResultsValidator);

                var linkBottomSearch = new LiteralControl("<a href=\"" + VirtualPathUtility.ToAbsolute("~/") + "\" class=\"newsearch\">New search</a>");
                Controls.Add(linkBottomSearch);

                var linkDisclaimer = new LiteralControl("<a href=\"" + HttpUtility.HtmlAttributeEncode(ConfigurationManager.AppSettings["SupportWithConfidenceDisclaimerUrl"]) + "\">Disclaimer</a>");
                Controls.Add(linkDisclaimer);
            }

        }

        #endregion


    }
}