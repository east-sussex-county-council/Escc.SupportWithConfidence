using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Escc.SupportWithConfidence.Controls
{
    /// <summary>
    /// Category Mapper takes a flat list of Categories via the constructor and returns a collection of Category objects 
    /// structured as a family tree via the Categories property.
    /// </summary>
    public partial class CategoryMapper
    {
        // Used to hold the top level categoeries who parentid = null
        private List<Category> _topCategories = new List<Category>();


        /// <summary>
        /// The constructor takes a flat list of categories, creating a collection of new category objects. Each category object
        /// after creation is checked to see if exists as a parent or a child category. The outcome of this loop will produce a collection of 
        /// Category objects structured as a family tree. Use the Categories property to access the category collection.
        /// </summary>
        /// <param name="dbcategories">DataSet</param>
        public CategoryMapper(IEnumerable<Category> categories)
        {
            if (categories == null) return;

            _topCategories = categories.Where(x => x.ParentId == 0).ToList();

            foreach (var category in categories)
            {
                if (category.ParentId == 0)
                {
                    category.ParentId = null;
                    continue;
                }

                var parentCategory = _topCategories.FirstOrDefault(x => x.CategoryId == category.ParentId);
                if (parentCategory == null) continue;
                parentCategory.Categories.Add(category);
            }
        }


        /// <summary>
        /// Returns a collection of Category objects ordered as a family tree.
        /// </summary>
        public IList<Category> Categories
        {
            get { return _topCategories; }
        }
    }
}