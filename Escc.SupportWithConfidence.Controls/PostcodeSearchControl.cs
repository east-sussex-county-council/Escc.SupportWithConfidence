using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services.Protocols;
using System.Web.UI;
using System.Web.UI.WebControls;
using Escc.FormControls.WebForms.AddressFinder;
using Escc.Net;
using Exceptionless;

namespace Escc.SupportWithConfidence.Controls
{
    public class PostcodeSearchControl : WebControl, INamingContainer
    {

        public PostcodeSearchControl()
            : base(HtmlTextWriterTag.Div)
        {
        }



        private AggregateEN _location = new AggregateEN();

        private string _buttonText = "Sort results";
        public string ButtonText
        {
            get { return _buttonText; }
            set { _buttonText = value; }

        }
        private string _postcodeText = "Sort by distance:";
        public string PostcodeText
        {
            get { return _postcodeText; }
            set { _postcodeText = value; }

        }

        private bool _showOnResults;

        public bool ShowOnResults
        {
            get { return _showOnResults; }
            set { _showOnResults = value; }
        }


        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            EnsureChildControls();

            if (_showOnResults)
            {
                var advice = new LiteralControl("<p class=\"intro\">To sort by those nearest to you, please enter your postcode or town.</p>");
                Controls.Add(advice);
            }

            var lblPostcode = new Label { Text = _postcodeText, ID = "lblPostcode", AssociatedControlID = "txbPostcode", CssClass = "formLabel" };
            var txbPostcode = new TextBox { ID = "txbPostcode", CssClass = "formControl describedby-tip", TextMode = TextBoxMode.SingleLine };
            txbPostcode.Attributes["aria-describedby"] = "data-protection";
            txbPostcode.Attributes["data-tip-positions"] = "bottom top";
            var btnSearch = new Button { ID = "btnSearch", Text = _buttonText, CssClass = "button" };

            Controls.Add(lblPostcode);
            Controls.Add(txbPostcode);
            Controls.Add(btnSearch);
            Controls.Add(new LiteralControl("<p id=\"data-protection\">We won't keep or share your postcode or town</p>"));

            if (HttpContext.Current.Request.Form[btnSearch.UniqueID] != null)
            {
                btnSearch_Click(btnSearch,null);
            }
        }

        void btnSearch_Click(object sender, EventArgs e)
        {

            var txbPostcode = (TextBox)FindControl("txbPostcode");
            txbPostcode.Text = HttpContext.Current.Request.Form[txbPostcode.UniqueID];

            if (txbPostcode.Text.Length > 0)
            {
                // Build get request with easting and northing plus existing parameters
                _location = Lookup(txbPostcode.Text);

                if (_location.Easting > 0)
                {
                   
                    // Store parameter values to add back in when we construct the new Uri
                    var parameters = HttpUtility.ParseQueryString(HttpContext.Current.Request.Url.Query);

                    if (String.IsNullOrEmpty(parameters["pc"]))
                    {
                        parameters.Add("pc", String.Empty);
                    }

                    if (String.IsNullOrEmpty(parameters["e"]))
                    {
                        parameters.Add("e", String.Empty);
                    }

                    if (String.IsNullOrEmpty(parameters["n"]))
                    {
                        parameters.Add("n", String.Empty);
                    }


                    var queryString = new StringBuilder();

                    foreach (var qsparameter in parameters.AllKeys)
                    {
                        switch (qsparameter)
                        {
                            case "cat":
                                queryString.Append("&cat=").Append(parameters[qsparameter]);
                          
                                break;
                            case "page":
                                queryString.Append("&page=").Append(parameters[qsparameter]);
                                break;
                            case "e":
                                queryString.Append("&e=").Append(_location.Easting.ToString(CultureInfo.InvariantCulture));
                                break;
                            case "n":
                                queryString.Append("&n=").Append(_location.Northing.ToString(CultureInfo.InvariantCulture));
                                break;
                            case "pc":
                                queryString.Append("&pc=").Append(txbPostcode.Text);
                                break;
                            case "w":
                                queryString.Append("&w=").Append(parameters[qsparameter]);
                                break;
                            case "s":
                                queryString.Append("&s=").Append(parameters[qsparameter]);
                                break;
                        }
                    }

                    HttpContext.Current.Response.Redirect("results.aspx?" + queryString.ToString(1, queryString.Length - 1));                   
                }
                else
                {

                    // Throw address no found error message
                    var noAddressFoundValidator = new CustomValidator
                        {
                            Display = ValidatorDisplay.None,
                            EnableClientScript = false,
                            IsValid = false,
                            ErrorMessage =
                                @"No results were found for: " + txbPostcode.Text +
                                @". If you entered a postcode please ensure this is a full postcode within East Sussex county. If you entered a town please check the spelling."
                        };


                    Controls.Add(noAddressFoundValidator);

                }

               

            }
        }


        private AggregateEN Lookup(string addressTerm)
        {
            //Look up Postcode Service Full or Partial]

            //Not Egif compliant, need to make the space (? ?) optional to allow for town names.

            var regLocation = new Regex("^[A-Z]{1,2}[0-9R][0-9A-Z]? ?[0-9][A-Z]{2}$", RegexOptions.IgnoreCase);

            using (var finder = new AddressFinder())
            {
                try
                {
                    finder.Credentials = new ConfigurationWebApiCredentialsProvider().CreateCredentials();

                    if (regLocation.IsMatch(addressTerm))
                    {
                        // Full postcode
                        return finder.AggregateEastingsAndNorthings(addressTerm);


                    }
                    // other wise use the place name finder
                    var ds = finder.GetPlaceNameData(addressTerm);
                    var dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        _location.Easting = Convert.ToInt32(dt.Rows[0]["Easting"]);
                        _location.Northing = Convert.ToInt32(dt.Rows[0]["Northing"]);

                        return _location;
                    }

                    // or try partial postcode if that looks plausible
                    if (addressTerm.StartsWith("BN", StringComparison.OrdinalIgnoreCase) || addressTerm.StartsWith("TN", StringComparison.OrdinalIgnoreCase) || addressTerm.StartsWith("RH", StringComparison.OrdinalIgnoreCase))
                    {
                        _location = finder.AggregateEastingsAndNorthingsPartialPostcode(addressTerm);
                        if (_location != null)
                        {
                            return _location;
                        }
                    }
                }
                catch (SoapException ex)
                {
                    if (!ex.Message.Contains("The postcode entered could not be found."))
                    {
                        ex.ToExceptionless().Submit();
                    }

                    return _location;
                }
                catch (Exception ex)
                {
                    ex.ToExceptionless().Submit();
                    return _location;
                }
            }

            return _location;
        }


    }
}


