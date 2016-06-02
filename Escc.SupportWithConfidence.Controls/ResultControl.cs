using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Escc.FormControls.WebForms.Validators;
using EsccWebTeam.NavigationControls;
using EsccWebTeam.SupportWithConfidence.Controls;

namespace Escc.SupportWithConfidence.Controls
{
    /// <summary>
    /// Class should include both the results and what was used to perform the search.
    /// </summary>
    public class ResultControl : WebControl, INamingContainer
    {

        #region fields


        protected ResultRepeater RptSupportResults = new ResultRepeater();

        protected PagingController PagingController = new PagingController();
        protected PagingBarControl PagingTop = new PagingBarControl();
        protected PagingBarControl PagingBottom = new PagingBarControl();



        #endregion

        public ResultControl()
            : base(HtmlTextWriterTag.Div)
        {
        }

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            EnsureChildControls();

            var controller = new SearchController(new WebApiProviderDataSource());

            IList<IResult> results = controller.GetResults();

            // if t then centre of town
            // if pc then postcode
            // if cat or st then alpha

            string sortOrder;
            string searchHeadingTerm;

            if (controller.QueryStringParameters.CategoryId > 0)
            {
                searchHeadingTerm = controller.CategoryHeading;
            }
            else if (controller.QueryStringParameters.ProviderSearchValue.Length > 0)
            {
                searchHeadingTerm = controller.QueryStringParameters.ProviderSearchValue;
            }
            else if (controller.QueryStringParameters.PostcodeSearchValue.Length > 0)
            {
                searchHeadingTerm = controller.QueryStringParameters.PostcodeSearchValue;
            }
            else
            {
                // No terms just click either button
                searchHeadingTerm = "everything";

            }



            if (controller.QueryStringParameters.PostcodeSearchValueIsTownName)
            {
                sortOrder = "by distance from 'centre of " + controller.QueryStringParameters.PostcodeSearchValue + "'";

            }
            else if (controller.QueryStringParameters.PostcodeSearchValue.Length > 0 & controller.QueryStringParameters.PostcodeSearchValueIsTownName == false)
            {
                sortOrder = "by distance from '" + controller.QueryStringParameters.PostcodeSearchValue + "'";
            }
            else
            {
                sortOrder = "alphabetically";
            }




            Controls.Add(new LiteralControl("<div class=\"content text-content\">"));

            var searchHeading = new LiteralControl("<h1>Search results for '" + searchHeadingTerm + "'</h1>");
            Controls.Add(searchHeading);

            ValidationSummary validationSummary = new EsccValidationSummary();
            validationSummary.DisplayMode = ValidationSummaryDisplayMode.BulletList;
            validationSummary.EnableClientScript = false;
            validationSummary.ShowSummary = true;

            Controls.Add(validationSummary);
            var formBoxOpenPostcode = new LiteralControl("<div class=\"form simple-form\">");
            Controls.Add(formBoxOpenPostcode);
            var sortHeading = new LiteralControl("<h2>Results are sorted " + sortOrder + "</h2>");
            Controls.Add(sortHeading);
            var postcodeSearchControl = new PostcodeSearchControl { ShowOnResults = true };
            Controls.Add(postcodeSearchControl);
            var formBoxClosePostcode = new LiteralControl("</div>");
            Controls.Add(formBoxClosePostcode);

            var linkToFeedback = new LiteralControl("<p><a href=\"http://www.eastsussex.gov.uk/forms/eforms.aspx?f=349&amp;p=1\" class=\"feedback\">Couldn’t find what you were looking for?</a></p>");
            Controls.Add(linkToFeedback);

            var linkTopSearch = new LiteralControl("<p><a href=\"search.aspx\" class=\"newsearch\">New search</a></p>");
            Controls.Add(linkTopSearch);

            if (results.Count > 0)
            {
                RptSupportResults.DataSource = results;
                RptSupportResults.DataBind();
                PagingController.TotalResults = controller.TotalResults;

                //Paging control

                PagingController.ResultsTextSingular = "provider";
                PagingController.ResultsTextPlural = "providers";
                PagingController.PageSize = 10;
                PagingController.EnableViewState = false;
                Controls.Add(PagingController);

                PagingTop.PagingController = PagingController;
                PagingTop.EnableViewState = false;
                PagingTop.CssClass = "infoBar";
                Controls.Add(PagingTop);



                //Populate the template of repeater (dgBuildings)
                Controls.Add(RptSupportResults);



                //Close paging control
                PagingBottom.PagingController = PagingController;
                PagingBottom.CssClass = "infoBar";
                Controls.Add(PagingBottom);

                var linkBottomSearch = new LiteralControl("<p><a href=\"search.aspx\" class=\"newsearch\">New search</a></p>");
                Controls.Add(linkBottomSearch);

                var linkDisclaimer = new LiteralControl("<p><a href=\"" + HttpUtility.HtmlAttributeEncode(ConfigurationManager.AppSettings["SupportWithConfidenceDisclaimerUrl"]) + "\">Disclaimer</a></p>");
                Controls.Add(linkDisclaimer);
            }
            else
            {
                var noResultsValidator = new CustomValidator
                    {
                        Display = ValidatorDisplay.None,
                        EnableClientScript = false,
                        IsValid = false,
                        ErrorMessage =
                            @"Unfortunately no results could be found that matched your search criteria. Please try searching again with different criteria."
                    };

                validationSummary.Controls.Add(noResultsValidator);

                searchHeading.Text = "<h1>No results found</h1>";
                sortHeading.Visible = false;

                postcodeSearchControl.Visible = false;


            }
            Controls.Add(new LiteralControl("</div>"));




        }
    }
}