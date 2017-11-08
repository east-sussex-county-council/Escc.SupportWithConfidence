using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Services.Protocols;
using Escc.Net;
using Exceptionless;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Escc.FormControls.WebForms.AddressFinder;

namespace Escc.SupportWithConfidence.Controls
{
    public class LocationSearcher
    {
        public Uri Search(string postcode)
        {
            if (postcode.Length > 0)
            {
                // Build get request with easting and northing plus existing parameters
                var location = Lookup(postcode);

                if (location.Easting > 0)
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
                                queryString.Append("&e=").Append(location.Easting.ToString(CultureInfo.InvariantCulture));
                                break;
                            case "n":
                                queryString.Append("&n=").Append(location.Northing.ToString(CultureInfo.InvariantCulture));
                                break;
                            case "pc":
                                queryString.Append("&pc=").Append(postcode);
                                break;
                            case "w":
                                queryString.Append("&w=").Append(parameters[qsparameter]);
                                break;
                            case "s":
                                queryString.Append("&s=").Append(parameters[qsparameter]);
                                break;
                        }
                    }

                    return new Uri(VirtualPathUtility.ToAbsolute("~/results.aspx?" + queryString.ToString(1, queryString.Length - 1)), UriKind.Relative);
                }
            }
            return null;
        }


        private AggregateEN Lookup(string addressTerm)
        {
            //Look up Postcode Service Full or Partial]

            //Not Egif compliant, need to make the space (? ?) optional to allow for town names.

            var regLocation = new Regex("^[A-Z]{1,2}[0-9R][0-9A-Z]? ?[0-9][A-Z]{2}$", RegexOptions.IgnoreCase);

            AggregateEN location = new AggregateEN();
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
                        location.Easting = Convert.ToInt32(dt.Rows[0]["Easting"]);
                        location.Northing = Convert.ToInt32(dt.Rows[0]["Northing"]);

                        return location;
                    }

                    // or try partial postcode if that looks plausible
                    if (addressTerm.StartsWith("BN", StringComparison.OrdinalIgnoreCase) || addressTerm.StartsWith("TN", StringComparison.OrdinalIgnoreCase) || addressTerm.StartsWith("RH", StringComparison.OrdinalIgnoreCase))
                    {
                        location = finder.AggregateEastingsAndNorthingsPartialPostcode(addressTerm);
                        if (location != null)
                        {
                            return location;
                        }
                    }
                }
                catch (SoapException ex)
                {
                    if (!ex.Message.Contains("The postcode entered could not be found."))
                    {
                        ex.ToExceptionless().Submit();
                    }

                    return location;
                }
                catch (Exception ex)
                {
                    ex.ToExceptionless().Submit();
                    return location;
                }
            }

            return location;
        }
    }
}