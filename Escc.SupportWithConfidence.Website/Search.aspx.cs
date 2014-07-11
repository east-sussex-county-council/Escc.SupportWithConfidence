using System;

namespace Escc.SupportWithConfidence.Website
{
    public partial class Search : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            //  btnSearch.Click += new EventHandler(btnSearch_Click);

        }

        /*    void btnSearch_Click(object sender, EventArgs e)
            {
                HttpContext.Current.Response.Redirect("results.aspx?st=" + txbProvider.Text);
            }*/

        protected void Page_Load(object sender, EventArgs e)
        {

        }


    }
}