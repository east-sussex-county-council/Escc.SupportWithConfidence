using System.Web.UI;
using System.Web.UI.WebControls;
using EsccWebTeam.SupportWithConfidence.Controls;

namespace Escc.SupportWithConfidence.Controls
{
    public class AdminProviderResultControl : WebControl, INamingContainer
    {

        #region Fields


        protected ResultRepeater RptSupportResults = new ResultRepeater();

        protected Escc.NavigationControls.WebForms.PagingController PagingController = new Escc.NavigationControls.WebForms.PagingController();
        protected Escc.NavigationControls.WebForms.PagingBarControl PagingTop = new Escc.NavigationControls.WebForms.PagingBarControl();
        protected Escc.NavigationControls.WebForms.PagingBarControl PagingBottom = new Escc.NavigationControls.WebForms.PagingBarControl();



        #endregion
        #region Overrideable Methods
        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            EnsureChildControls();
            var pageIndex = 1;
            const int pageSize = 10;
            var mapper = new ProviderMapper(new SqlServerProviderDataRepository());

            if (System.Web.HttpContext.Current.Request.QueryString["page"] != null)
            {
                pageIndex = System.Convert.ToInt16(System.Web.HttpContext.Current.Request.QueryString["page"]);
                mapper.Map(pageIndex, pageSize);
            }
            else
            {
                mapper.Map(pageIndex, pageSize);
            }






            Controls.Add(new LiteralControl("<h1 class=\"text\">Update a provider's information</h1>"));
            Controls.Add(new LiteralControl("<p class=\"text\">Select a provider below by clicking on the provider's name.</p>"));

            RptSupportResults.DataSource = mapper.Providers;
            RptSupportResults.DataBind();
            PagingController.TotalResults = mapper.TotalResults;

            //Paging control

            PagingController.ResultsTextSingular = "provider";
            PagingController.ResultsTextPlural = "providers";
            PagingController.PageSize = 10;
            PagingController.EnableViewState = false;
            Controls.Add(PagingController);

            PagingTop.PagingController = PagingController;
            PagingTop.EnableViewState = false;
            PagingTop.CssClass = "roundedBox infoBar";
            Controls.Add(PagingTop);




            Controls.Add(RptSupportResults);



            //Close paging control
            PagingBottom.PagingController = PagingController;
            PagingBottom.CssClass = "roundedBox infoBar";
            Controls.Add(PagingBottom);



        }
        #endregion
    }
}
