using System.Collections.Generic;

namespace Escc.SupportWithConfidence.Controls
{
    /// <summary>
    /// Sorts categories alphabetically, except that Personal Assistants always come first
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IComparer{Escc.SupportWithConfidence.Controls.Category}" />
    public class CategorySorter : IComparer<Category>
    {
        public int Compare(Category x, Category y)
        {
            if (x.Description.ToUpperInvariant() == "PERSONAL ASSISTANT") return -1;
            if (y.Description.ToUpperInvariant() == "PERSONAL ASSISTANT") return 1;
            return x.Description.CompareTo(y.Description);
        }
    }
}