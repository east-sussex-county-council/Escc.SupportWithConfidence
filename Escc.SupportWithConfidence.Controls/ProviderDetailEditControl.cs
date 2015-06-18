using System;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Escc.FormControls.WebForms.Validators;
using EsccWebTeam.Data.Web;

namespace Escc.SupportWithConfidence.Controls
{
    public class ProviderDetailEditControl : WebControl, INamingContainer
    {
        private IProviderDataRepository _repository;

        #region Override Methods

        /// <summary>
        ///     Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            EnsureChildControls();


            int reference;
            int.TryParse(HttpContext.Current.Request.QueryString["ref"], out reference);

            _repository = new SqlServerProviderDataRepository();
            var proMapper = new ProviderMapper(_repository);


            proMapper.GetProvider(reference);

            if (proMapper.Providers.Count > 0)
            {
                Controls.Add(new LiteralControl("<div class=\"text\">"));

                Provider p = proMapper.Providers[0];

                var html = new StringBuilder();
                Controls.Add(new LiteralControl("<h1>" + p.ProviderName + "</h1>"));

                ValidationSummary validationSummary = new EsccValidationSummary();
                validationSummary.DisplayMode = ValidationSummaryDisplayMode.BulletList;
                validationSummary.EnableClientScript = false;
                validationSummary.ShowSummary = true;

                Controls.Add(validationSummary);


                Controls.Add(new ImageUploader
                    {
                        ID = "imageuploadercontrol",
                        FileDataId = p.PhotographId,
                        FlareId = p.FlareId
                    });


                html.Append("<h2>Contact details</h2>");


                html.Append("<div class=\"vcard\">");
                if (p.PublishAddress)
                {
                    html.Append("<p>");
                    if (p.ContactName.Length > 0)
                    {
                        html.Append("<span class=\"n\">" + p.ContactName + "</span><br>");
                    }
                    else
                    {
                        html.Append("<span class=\"org\">" + p.ProviderName + "</span><br>");
                    }
                    html.Append("<span class=\"location adr\">");
                    if (p.Address.Paon.Length > 4)
                    {
                        html.Append("<span class=\"street-address\">" + p.Address.Paon + " " + p.Address.Saon + "<br />" +
                                    p.Address.StreetName + "</span><br />");
                    }
                    else
                    {
                        html.Append("<span class=\"street-address\">" + p.Address.Paon + " " + p.Address.Saon + " " +
                                    p.Address.StreetName + "</span><br />");
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
                    html.Append("<br>");
                    html.Append("<span class=\"tel\">");
                    html.Append("<span class=\"aural\">");
                    html.Append("<span class=\"type\"></span> ");
                    html.Append("</span>");
                    html.Append("Mobile: <span class=\"value\">" + p.Mobile + "</span>");
                    html.Append("</span>");
                }

                if (p.Email.Trim().Length > 0)
                {
                    html.Append("<br>");
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
                    html.Append("<p>" + p.Availability + "</P>");
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
                    html.Append("<h2>Criminal records information</h2>");
                    html.Append("<p>" + p.Crb + "</p>");
                }

                if (p.CrbCheckDate.Trim().Length > 0)
                {
                    html.Append("<h2>Date of last Criminal Records check</h2>");
                    html.Append("<p>" + p.CrbCheckDate + "</p>");
                }


                if (p.CqcCheckDate.Trim().Length > 0)
                {
                    html.Append("<h2>Date of Care Quality Commission (CQC) Registration</h2>");
                    html.Append("<p>" + p.CqcCheckDate + "</p>");
                }


                if (p.BwcMember)
                {
                    html.Append(
                        "<img src=\"/managewebsite/supportwithconfidence/images/bwc_logo.jpg\" alt=\"Buy with Confidence Member\" class=\"bwclogo\">");
                }


                Controls.Add(new LiteralControl(html.ToString()));
                Controls.Add(new LiteralControl("</div>"));

                // DataTable dt = GetData();
                var editHeading = new LiteralControl("<h2>Editable bits</h2>");

                var formBoxOpen = new LiteralControl("<div class=\"formBox\">");
                var formPartOpenExperience = new LiteralControl("<div class=\"formPart\">");
                var lblExperience = new Label
                    {
                        ID = "lblExperience",
                        CssClass = "formLabel",
                        AssociatedControlID = "txbExperience",
                        Text = @"Experience:"
                    };
                var txbExperience = new TextBox
                    {
                        ID = "txbExperience",
                        TextMode = TextBoxMode.MultiLine,
                        Text = p.Experience,
                        CssClass = "formControl"
                    };
                var formPartCloseExperience = new LiteralControl("</div>");

                var formPartOpenBackground = new LiteralControl("<div class=\"formPart\">");
                var lblBackground = new Label
                    {
                        ID = "lblBackground",
                        CssClass = "formLabel",
                        AssociatedControlID = "txbBackground",
                        Text = @"Background:"
                    };
                var txbBackground = new TextBox
                    {
                        ID = "txbBackground",
                        TextMode = TextBoxMode.MultiLine,
                        Text = p.Background,
                        CssClass = "formControl"
                    };
                var formPartCloseBackground = new LiteralControl("</div>");

                var formPartOpenExpertise = new LiteralControl("<div class=\"formPart\">");
                var lblExpertise = new Label
                    {
                        ID = "lblExpertise",
                        CssClass = "formLabel",
                        AssociatedControlID = "txbExpertise",
                        Text = @"Expertise:"
                    };
                var txbExpertise = new TextBox
                    {
                        ID = "txbExpertise",
                        TextMode = TextBoxMode.MultiLine,
                        Text = p.Expertise,
                        CssClass = "formControl"
                    };
                var formPartCloseExpertise = new LiteralControl("</div>");

                var formPartOpenAccreditation = new LiteralControl("<div class=\"formPart\">");
                var lblAccreditation = new Label
                    {
                        ID = "lblAccreditation",
                        CssClass = "formLabel",
                        AssociatedControlID = "txbAccreditation",
                        Text = @"Accreditation:"
                    };
                var txbAccreditation = new TextBox
                    {
                        ID = "txbAccreditation",
                        TextMode = TextBoxMode.MultiLine,
                        Text = p.Accreditation,
                        CssClass = "formControl"
                    };
                var formPartCloseAccreditation = new LiteralControl("</div>");

                var formPartOpenServices = new LiteralControl("<div class=\"formPart\">");
                var lblServices = new Label
                    {
                        ID = "lblServices",
                        CssClass = "formLabel",
                        AssociatedControlID = "txbServices",
                        Text = @"Services:"
                    };
                var txbServices = new TextBox
                    {
                        ID = "txbServices",
                        TextMode = TextBoxMode.MultiLine,
                        Text = p.Services,
                        CssClass = "formControl"
                    };
                var formPartCloseServices = new LiteralControl("</div>");

                var formPartOpenCosts = new LiteralControl("<div class=\"formPart\">");
                var lblCosts = new Label
                    {
                        ID = "lblCosts",
                        CssClass = "formLabel",
                        AssociatedControlID = "txbCosts",
                        Text = @"Costs:"
                    };
                var txbCosts = new TextBox
                    {
                        ID = "txbCosts",
                        TextMode = TextBoxMode.MultiLine,
                        Text = p.Costs,
                        CssClass = "formControl"
                    };
                var formPartCloseCosts = new LiteralControl("</div>");

                var formPartOpenCrb = new LiteralControl("<div class=\"formPart\">");
                var lblCrb = new Label
                    {
                        ID = "lblCrb",
                        CssClass = "formLabel",
                        AssociatedControlID = "txbCrb",
                        Text = @"Criminal records information:"
                    };
                var txbCrb = new TextBox
                    {
                        ID = "txbCrb",
                        TextMode = TextBoxMode.MultiLine,
                        Text = p.Crb,
                        CssClass = "formControl"
                    };
                var formPartCloseCrb = new LiteralControl("</div>");


                var formPartOpenPublishToWeb = new LiteralControl("<div class=\"radioButtonList checkbox\">");
                var cxbPublishToWeb = new CheckBox
                    {
                        ID = "cxbPublishToWeb",
                        Checked = Convert.ToBoolean(p.PublishToWeb),
                        Text = @"Publish on website"
                    };
                var formPartClosePublishToWeb = new LiteralControl("</div>");

                var formBoxClose = new LiteralControl("</div>");


                var formPartOpenButton = new LiteralControl("<div class=\"formButtons\">");
                var btnSave = new Button {ID = "btnSave", CssClass = "button", Text = @"Save"};
                var formPartCloseButton = new LiteralControl("</div>");

                btnSave.Click += btnSave_Click;


                Controls.Add(new LiteralControl("<div class=\"form short-form\">"));
                Controls.Add(editHeading);

                Controls.Add(formBoxOpen);
                Controls.Add(formPartOpenExperience);
                Controls.Add(lblExperience);
                Controls.Add(txbExperience);
                Controls.Add(formPartCloseExperience);


                Controls.Add(formPartOpenBackground);
                Controls.Add(lblBackground);
                Controls.Add(txbBackground);
                Controls.Add(formPartCloseBackground);


                Controls.Add(formPartOpenExpertise);
                Controls.Add(lblExpertise);
                Controls.Add(txbExpertise);
                Controls.Add(formPartCloseExpertise);


                Controls.Add(formPartOpenAccreditation);
                Controls.Add(lblAccreditation);
                Controls.Add(txbAccreditation);
                Controls.Add(formPartCloseAccreditation);

                Controls.Add(formPartOpenServices);
                Controls.Add(lblServices);
                Controls.Add(txbServices);
                Controls.Add(formPartCloseServices);

                Controls.Add(formPartOpenCosts);
                Controls.Add(lblCosts);
                Controls.Add(txbCosts);
                Controls.Add(formPartCloseCosts);

                Controls.Add(formPartOpenCrb);
                Controls.Add(lblCrb);
                Controls.Add(txbCrb);
                Controls.Add(formPartCloseCrb);


                Controls.Add(formPartOpenPublishToWeb);

                Controls.Add(cxbPublishToWeb);
                Controls.Add(formPartClosePublishToWeb);

                Controls.Add(formBoxClose);


                //  byte[] myByteArray = ImageToByte2(p.Photograph);


                if (HttpContext.Current.Request.QueryString["success"] != null)
                {
                    if (HttpContext.Current.Request.QueryString["success"] == "1")
                    {
                        var saveSuccessValidator = new CustomValidator
                            {
                                Display = ValidatorDisplay.None,
                                EnableClientScript = false,
                                IsValid = false,
                                ErrorMessage = @"Record updated, the providers information was saved sucessfully."
                            };


                        validationSummary.Controls.Add(saveSuccessValidator);
                    }
                    else
                    {
                        var saveFailureValidator = new CustomValidator
                            {
                                Display = ValidatorDisplay.None,
                                EnableClientScript = false,
                                IsValid = false,
                                ErrorMessage =
                                    @"Save failed, the technical guys have been notified and will be looking into the issue."
                            };

                        validationSummary.Controls.Add(saveFailureValidator);
                    }
                }

                Controls.Add(formPartOpenButton);
                Controls.Add(btnSave);
                Controls.Add(formPartCloseButton);
                Controls.Add(new LiteralControl("</div>"));


                var linkBottomSearch =
                    new LiteralControl(
                        "<p class=\"text\"><a href=\"providers.aspx\" class=\"bottomsearch\">Find a provider</a></p>");
                Controls.Add(linkBottomSearch);

                //html.Append("<img src='person.jpg'alt='person' class='photo' />");
                if (HttpContext.Current.Request.HttpMethod == "GET")
                {
                    BindData();
                }
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

                var linkBottomSearch =
                    new LiteralControl("<a href=\"providers.aspx\" class=\"newsearch\">Find a provider</a>");
                Controls.Add(linkBottomSearch);
            }
        }

        private void BindData()
        {
            if (HttpContext.Current.Request.QueryString["ref"] != null)
            {
                int id = Convert.ToInt32(HttpContext.Current.Request.QueryString["ref"]);
                DataSet ds = _repository.GetProvider(id);

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    var txbExperience = (TextBox) FindControl("txbExperience");
                    txbExperience.Text = row["Experience"].ToString();

                    var txbExpertise = (TextBox) FindControl("txbExpertise");
                    txbExpertise.Text = row["Expertise"].ToString();

                    var txbBackground = (TextBox) FindControl("txbBackground");
                    txbBackground.Text = row["Background"].ToString();

                    var txbAccreditation = (TextBox) FindControl("txbAccreditation");
                    txbAccreditation.Text = row["Accreditation"].ToString();

                    var txbServices = (TextBox) FindControl("txbServices");
                    txbServices.Text = row["Services"].ToString();

                    var txbCosts = (TextBox) FindControl("txbCosts");
                    txbCosts.Text = row["Costs"].ToString();

                    var txbCrb = (TextBox) FindControl("txbCrb");
                    txbCrb.Text = row["Crb"].ToString();

                    var cxbPublishToWeb = (CheckBox) FindControl("cxbPublishToWeb");
                    cxbPublishToWeb.Checked = Convert.ToBoolean(row["PublishToWeb"]);
                }
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

                var linkBottomSearch = new LiteralControl("<a href=\"search.aspx\" class=\"newsearch\">New search</a>");
                Controls.Add(linkBottomSearch);

                var linkDisclaimer =
                    new LiteralControl(
                        "<a href=\"" + HttpUtility.HtmlAttributeEncode(ConfigurationManager.AppSettings["SupportWithConfidenceDisclaimerUrl"]) + "\">Disclaimer</a>");
                Controls.Add(linkDisclaimer);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (HttpContext.Current.Request.HttpMethod == "POST")
            {
                if (HttpContext.Current.Request.QueryString["ref"] != null)
                {
                    int id = Convert.ToInt32(HttpContext.Current.Request.QueryString["ref"]);

                    var txbExperience = (TextBox) FindControl("txbExperience");
                    string experience = txbExperience.Text;

                    var txbExpertise = (TextBox) FindControl("txbExpertise");
                    string expertise = txbExpertise.Text;

                    var txbBackground = (TextBox) FindControl("txbBackground");
                    string background = txbBackground.Text;

                    var txbAccreditation = (TextBox) FindControl("txbAccreditation");
                    string accreditation = txbAccreditation.Text;

                    var txbServices = (TextBox) FindControl("txbServices");
                    string services = txbServices.Text;

                    var txbCosts = (TextBox) FindControl("txbCosts");
                    string costs = txbCosts.Text;

                    var txbCrb = (TextBox) FindControl("txbCrb");
                    string crb = txbCrb.Text;

                    var cxbPublishToWeb = (CheckBox) FindControl("cxbPublishToWeb");
                    bool pubish = cxbPublishToWeb.Checked;

                    var imageuploadercontrol = (ImageUploader) FindControl("imageuploadercontrol");
                    imageuploadercontrol.Save();

                    var repo = new SqlServerProviderDataRepository();
                    bool success = repo.SaveProviderInformation(id, experience, expertise, background, accreditation,
                                                                      services, costs, crb, pubish);

                    if (success)
                    {
                        string queryString = HttpContext.Current.Request.Url.Query;
                        if (HttpContext.Current.Request.Url.Query.Contains("success="))
                        {
                            var qs = Iri.RemoveQueryStringParameter(HttpContext.Current.Request.Url, "success");
                            queryString = qs.Query;
                        }
                        HttpContext.Current.Response.Redirect("provider.aspx" + queryString + "&success=1");
                    }
                }
                else
                {
                    string queryString = HttpContext.Current.Request.Url.Query;
                    if (HttpContext.Current.Request.Url.Query.Contains("success="))
                    {
                        var qs = Iri.RemoveQueryStringParameter(HttpContext.Current.Request.Url, "success");
                        queryString = qs.Query;
                    }
                    HttpContext.Current.Response.Redirect("provider.aspx" + queryString + "&success=0");
                }
            }
        }

        #endregion

        #region Supporting Methods

        [Obsolete("Need to update code to call EsccWebTeam.Data.Web.Iri.RemoveParameterFromQueryString")]
        private string RemoveParameter(string qs, string param)
        {
            // if supplied querystring starts with ?, remove it
            if (qs.StartsWith("?")) qs = qs.Substring(1);

            // split supplied querystring into sections
            qs = qs.Replace("&amp;", "&");
            string[] qsBits = qs.Split('&');

            //rebuild query string without its parameter= value
            var newQs = new StringBuilder();

            for (int i = 0; i < qsBits.Length; i++)
            {
                string[] paramBits = qsBits[i].Split('=');

                if ((paramBits[0] != param) && (paramBits.Length > 1))
                {
                    if (newQs.Length > 0) newQs.Append("&");
                    newQs.Append(paramBits[0]).Append("=").Append(paramBits[1]);
                }
            }

            // get querystring ready for new parameter
            //if (newQS.Length > 0) newQS.Append("&");
            //else newQS.Append("?");

            string completeQs = newQs.ToString();
            if (!completeQs.StartsWith("?")) completeQs = "?" + completeQs;

            return completeQs;
        }

        #endregion
    }
}