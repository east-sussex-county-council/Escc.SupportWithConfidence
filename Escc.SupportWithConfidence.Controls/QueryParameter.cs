using System;
using System.Text;
using System.Web;
using EsccWebTeam.SupportWithConfidence.Controls;
using Exceptionless;

namespace Escc.SupportWithConfidence.Controls
{
    public class QueryParameter
    {
        public bool PostcodeSearchValueIsTownName { get; set; }

        public int PageSize { get; set; }

        public int Easting { get; set; }

        public int Northing { get; set; }

        public string PostcodeSearchValue { get; set; }

        public string ProviderSearchValue { get; set; }

        public int CategoryId { get; set; }

        public int CurrentResultPage { get; set; }

        public int ProviderReferenceId { get; set; }

        private SearchCall _searchCall;

        public QueryParameter()
        {
            CurrentResultPage = 1;
            ProviderSearchValue = string.Empty;
            PostcodeSearchValue = string.Empty;
            PageSize = 10;
        }

        public SearchCall DataSearchCall
        {
            get { return _searchCall; }
        }

        public void Process()
        {
            var request = HttpContext.Current.Request;
            var querystrings = request.QueryString;


            foreach (var querystring in querystrings)
                if (querystring != null)
                    switch (querystring.ToString())
                    {
                        #region Category

                        case "cat":
                            try
                            {
                                int cat;
                                var category = request.QueryString[(string)querystring];
                                var result = int.TryParse(category, out cat);
                                CategoryId = result == false ? 0 : cat;

                            }
                            catch (OverflowException oEx)
                            {
                                oEx.ToExceptionless().Submit();
                            }
                            catch (FormatException fEx)
                            {
                                fEx.ToExceptionless().Submit();
                            }
                            catch (Exception ex)
                            {
                                ex.ToExceptionless().Submit();
                            }
                            break;

                        #endregion

                        #region Postcode

                        case "pc":
                            PostcodeSearchValue = request.QueryString[querystring.ToString()];
                            break;

                        #endregion

                        #region SearchTerm

                        case "s":
                            try
                            {
                                ProviderSearchValue = request.QueryString[querystring.ToString()];
                            }
                            catch (OverflowException oEx)
                            {
                                oEx.ToExceptionless().Submit();
                            }
                            catch (FormatException fEx)
                            {
                                fEx.ToExceptionless().Submit();
                            }
                            catch (Exception ex)
                            {
                                ex.ToExceptionless().Submit();
                            }
                            break;

                        #endregion

                        #region PageNumber

                        case "page":
                            try
                            {
                                CurrentResultPage = Convert.ToInt16(request.QueryString[querystring.ToString()]);
                            }
                            catch (OverflowException)
                            {
                                CurrentResultPage = 1;
                            }
                            catch (FormatException)
                            {
                                CurrentResultPage = 1;
                            }

                            break;

                        #endregion

                        #region Easting

                        case "e":
                            try
                            {
                                Easting = Convert.ToInt32(request.QueryString[querystring.ToString()]);
                            }
                            catch (OverflowException oEx)
                            {
                                oEx.ToExceptionless().Submit();
                            }
                            catch (FormatException fEx)
                            {
                                fEx.ToExceptionless().Submit();
                            }
                            catch (Exception ex)
                            {
                                ex.ToExceptionless().Submit();
                            }
                            break;

                        #endregion

                        #region Northing

                        case "n":
                            try
                            {
                                Northing = Convert.ToInt32(request.QueryString[querystring.ToString()]);
                            }
                            catch (OverflowException oEx)
                            {
                                oEx.ToExceptionless().Submit();
                            }
                            catch (FormatException fEx)
                            {
                                fEx.ToExceptionless().Submit();
                            }
                            catch (Exception ex)
                            {
                                ex.ToExceptionless().Submit();
                            }
                            break;

                        #endregion

                        #region Town

                        case "w":
                            try
                            {
                                PostcodeSearchValueIsTownName = request.QueryString[querystring.ToString()] == "1";
                            }
                            catch (OverflowException oEx)
                            {
                                oEx.ToExceptionless().Submit();
                            }
                            catch (FormatException fEx)
                            {
                                fEx.ToExceptionless().Submit();
                            }
                            catch (Exception ex)
                            {
                                ex.ToExceptionless().Submit();
                            }
                            break;

                        #endregion

                        default:
                            // No results will be returned and results control will handle telling the user
                            CategoryId = 0;
                            Easting = 0;
                            Northing = 0;
                            CurrentResultPage = 1;
                            ProviderSearchValue = string.Empty;
                            break;
                    }

            _searchCall = CategoryId > 0 ? SearchCall.Category : SearchCall.Provider;
        }

        public override string ToString()
        {

            var query = new StringBuilder();
            if (CategoryId > 0)
            {
                query.Append("cat=" + CategoryId);
            }
            return query.ToString();
        }
    }
}