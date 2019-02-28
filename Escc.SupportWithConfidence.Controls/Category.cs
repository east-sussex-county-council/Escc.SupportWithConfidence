using System.Collections.Generic;

namespace Escc.SupportWithConfidence.Controls
{
    /// <summary>
    /// Category models the fields in the Category table and can be used to hold a parent or child category.
    /// </summary>
    public class Category
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public int ParentId { get; set; }

        public int Depth { get; set; }

        public bool IsActive { get; set; }

        public int Sequence { get; set; }

        /// <summary>
        /// Returns a collection of child categories
        /// </summary>
        public List<Category> Categories { get; set; } = new List<Category>();
    }
}