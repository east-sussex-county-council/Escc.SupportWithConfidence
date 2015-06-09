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
using EsccWebTeam.Data.Web;
using Microsoft.ApplicationBlocks.ExceptionManagement;

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
            var txbPostcode = new TextBox { ID = "txbPostcode", CssClass = "formControl", TextMode = TextBoxMode.SingleLine };
            var btnSearch = new Button { ID = "btnSearch", Text = _buttonText, CssClass = "button" };

            btnSearch.Click += btnSearch_Click;

            Controls.Add(lblPostcode);
            Controls.Add(txbPostcode);
            Controls.Add(btnSearch);
        }

        [Obsolete("Need to update code to call EsccWebTeam.Data.Web.Iri.RemoveParameterFromQueryString")]
        public string RemoveParameter(string qs, string param)
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

        void btnSearch_Click(object sender, EventArgs e)
        {

            var txbPostcode = (TextBox)FindControl("txbPostcode");

 Uri url = HttpContext.Current.Request.Url;
            if (txbPostcode.Text.Length > 0)
            {
                // Build get request with easting and northing plus existing parameters
                _location = Lookup(txbPostcode.Text);

                if (_location.Easting > 0)
                {
                   
                    // LEGACY REMOVE BEFORE COMMIT
                    string queryString = HttpContext.Current.Request.Url.Query;
                   
                    
                    // Store parameter values to add back in when we construct the new Uri
                    var parameters = Iri.SplitQueryString(queryString);
                   


                    foreach (var qsparameter in parameters)
                    {
                        url = Iri.RemoveQueryStringParameter(url, qsparameter.Key);
                    }

                    if (!parameters.ContainsKey("pc"))
                        parameters.Add("pc", txbPostcode.Text);

                    if (!parameters.ContainsKey("e"))
                        parameters.Add("e", _location.Easting.ToString(CultureInfo.InvariantCulture));

                    if (!parameters.ContainsKey("n"))
                        parameters.Add("n",_location.Northing.ToString(CultureInfo.InvariantCulture));


                    url = new Uri(url + "?");

                    foreach (var qsparameter in parameters)
                    {
                        switch (qsparameter.Key)
                        {
                            case "cat":
                               url = new Uri(url + "cat=" + qsparameter.Value);
                          
                                break;
                            case "page":
                               url = new Uri(url + "page=" + qsparameter.Value);
                                break;
                            case "e":
                                url = new Uri(url + "e=" + _location.Easting.ToString(CultureInfo.InvariantCulture));
                                break;
                            case "n":
                                  url = new Uri(url + "n=" + _location.Northing.ToString(CultureInfo.InvariantCulture));
                                break;
                            case "pc":
                                url = new Uri(url + "pc=" + txbPostcode.Text);
                                break;
                            case "w":
                                url = new Uri(url + "w=" + qsparameter.Value);
                                break;
                            case "s":
                                url = new Uri(url + "s=" + qsparameter.Value);
                                break;
                        }

                        url = new Uri(url + "&");
                    }


                    var pathUrl = url.AbsoluteUri;
                    var position = pathUrl.LastIndexOf("&", StringComparison.Ordinal);
                    var newPath =   pathUrl.Remove(position);


                    HttpContext.Current.Response.Redirect(newPath.Replace("search", "results"));
                   
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
            else
            {
                HttpContext.Current.Response.Redirect( HttpContext.Current.Request.Url.ToString());
            }
        }


        private AggregateEN Lookup(string addressTerm)
        {
            //Look up Postcode Service Full or Partial]

            //Not Egif compliant, need to make the space (? ?) optional to allow for town names.

            var regLocation = new Regex("^[A-Z]{1,2}[0-9R][0-9A-Z]? ?[0-9][A-Z]{2}$", RegexOptions.IgnoreCase);



            var finder = new AddressFinder();

            try
            {
                if (regLocation.IsMatch(addressTerm))
                {
                    // Full postcode
                    DataAccess.PostcodeCount();
                    return finder.AggregateEastingsAndNorthings(addressTerm);


                }
                // other wise use the place name finder
                DataAccess.PostcodeCount();
                var ds = finder.GetPlaceNameData(addressTerm);
                var dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    _location.Easting = Convert.ToInt32(dt.Rows[0]["Easting"]);
                    _location.Northing = Convert.ToInt32(dt.Rows[0]["Northing"]);

                    return _location;
                }
                _location = finder.AggregateEastingsAndNorthingsPartialPostcode(addressTerm);
                if (_location != null)
                {
                    return _location;
                }
            }
            catch (SoapException)
            {
                //ExceptionManager.Publish(soapEx);
                //Can't find an validate address information
                return _location;


            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                return _location;
            }

            return _location;
        }


    }
}


