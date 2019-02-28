using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Escc.SupportWithConfidence.Controls
{
    /// <summary>
    /// Category Mapper takes a datatable of Categories via the constructor and returns a collection of Category objects 
    /// structured as a family tree via the Categories property.
    /// </summary>
    public partial class CategoryMapper
    {
        // Used to hold the top level categoeries who parentid = null
        private List<Category> _topCategories = new List<Category>();


        /// <summary>
        /// The constructor takes a datatable and loops over ever row, creating a collection of new category objects. Each category object
        /// after creation is checked to see if exists as a parent or a child category. The outcome of this loop will produce a collection of 
        /// Category objects structured as a family tree. Use the Categories property to access the category collection.
        /// </summary>
        /// <param name="dbcategories">DataSet</param>
        public CategoryMapper(DataSet dbcategories)
        {
            if (dbcategories == null) return;

            var categories = new List<Category>();

            foreach (DataRow dbcategory in dbcategories.Tables[0].Rows)
            {
                categories.Add(new Category
                {
                    Id = Convert.ToInt16(dbcategory["CategoryId"]),
                    Code = dbcategory["Code"].ToString(),
                    Description = dbcategory["Description"].ToString(),
                    ParentId =
                            dbcategory["ParentId"] == DBNull.Value
                                ? 0
                                : Convert.ToInt16(dbcategory["ParentId"]),
                    Depth = Convert.ToInt16(dbcategory["Depth"]),
                    IsActive = Convert.ToBoolean(dbcategory["IsActive"]),
                    Sequence = Convert.ToInt32(dbcategory["Sequence"])
                }
                );
            }

            _topCategories = categories.Where(x => x.ParentId == 0).ToList();

            foreach (var category in categories)
            {
                if (category.ParentId == 0) continue;

                var parentCategory = _topCategories.FirstOrDefault(x => x.Id == category.ParentId);
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