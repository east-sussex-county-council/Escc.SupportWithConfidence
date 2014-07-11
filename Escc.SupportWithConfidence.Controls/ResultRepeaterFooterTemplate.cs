using System.Web.UI;

namespace Escc.SupportWithConfidence.Controls
{

    /// <summary>
    /// End the Repeater by closing the unordered list
    /// </summary>
   public class ResultRepeaterFooterTemplate :ITemplate
    {
        #region ITemplate Members

        /// <summary>
        /// End the Repeater by closing the unordered list
        /// </summary>
        public void InstantiateIn(Control container)
        {
            container.Controls.Add(new LiteralControl("</div>"));
        }

        #endregion
    }
}
