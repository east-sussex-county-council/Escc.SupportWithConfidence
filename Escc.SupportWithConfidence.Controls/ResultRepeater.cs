using System.Web.UI.WebControls;
using Escc.SupportWithConfidence.Controls;

namespace EsccWebTeam.SupportWithConfidence.Controls
{
   public class ResultRepeater: Repeater
    {
       

       protected override void CreateChildControls()
       {
           base.CreateChildControls();
           EnsureChildControls();

           HeaderTemplate = new ResultRepeaterHeaderTemplate();
           ItemTemplate = new ResultRepeaterItemTemplate();
           FooterTemplate = new ResultRepeaterFooterTemplate();
       }


    }
}
