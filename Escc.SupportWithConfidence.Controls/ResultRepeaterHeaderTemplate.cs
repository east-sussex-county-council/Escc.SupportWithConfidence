using System.Web.UI;

namespace EsccWebTeam.SupportWithConfidence.Controls
{
    /// <summary>
    /// Begin the Repeater by opening an unordered list
    /// </summary>
    class ResultRepeaterHeaderTemplate: ITemplate
    {
        #region ITemplate Members

        /// <summary>
        /// Begin the Repeater by opening an unordered list
        /// </summary>
        public void InstantiateIn(Control container)
        {
            container.Controls.Add(new LiteralControl("<div>"));
        }

        #endregion
    }
}
