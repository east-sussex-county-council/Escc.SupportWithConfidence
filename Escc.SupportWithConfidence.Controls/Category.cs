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

        private List<Category> _categories = new List<Category>();
        
        /// <summary>
        /// Returns a collection of child categories
        /// </summary>
        public List<Category> Categories
        {
            get { return _categories; }
            set{_categories = value; }
        }

        /// <summary>
        /// Find a category based on id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Category FindCategory(int id)
        {
            return FindCategory(this, id);
        }

        /// <summary>
        /// Find child categories
        /// </summary>
        /// <param name="category"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private static Category FindCategory(Category category, int id)
        {
            if (category.Id == id)
            {
                return category;
            }
            Category returnCategory = null;
           foreach (var child in category.Categories)
            {
                returnCategory = child.FindCategory(id);
                if (returnCategory != null)
                {
                    break;
                }
            }
            return returnCategory;
        }
    }
}