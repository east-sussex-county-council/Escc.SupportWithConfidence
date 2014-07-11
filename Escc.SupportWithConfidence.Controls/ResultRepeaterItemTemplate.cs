using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Escc.SupportWithConfidence.Controls
{
    public class ResultRepeaterItemTemplate : ITemplate 
    {
        
        public void InstantiateIn(Control container)
        {
            var listItem = new HtmlGenericControl("div");
            listItem.DataBinding += BindItem;
            container.Controls.Add(listItem);
        }

        private static void BindItem(object sender, EventArgs e)
        {
            var listItem = (HtmlGenericControl)sender;
            listItem.Attributes.Add("class", "x_form-result");
            var container = (RepeaterItem)listItem.NamingContainer;
            var result = ((IResult)container.DataItem);
            listItem.InnerHtml = result.View();
        }
    }
}
