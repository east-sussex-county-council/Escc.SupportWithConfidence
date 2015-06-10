using System.Configuration;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using EsccWebTeam.SupportWithConfidence.Controls;

namespace Escc.SupportWithConfidence.Controls
{
    /// <summary>
    /// Category Control takes a list of categories and produces a set of unordered lists depending on the tree structure of the collection.
    /// </summary>
    public class CategorySearchControl : WebControl, INamingContainer
    {
        private readonly IProviderDataSource _dataSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategorySearchControl"/> class.
        /// </summary>
        public CategorySearchControl()
        {
            _dataSource = new SqlServerProviderDataSource();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CategorySearchControl"/> class.
        /// </summary>
        /// <param name="dataSource">The data source.</param>
        public CategorySearchControl(IProviderDataSource dataSource)
        {
            _dataSource = dataSource;
        }

        private bool _hasProvider;

        public bool HasProvider
        {
            get { return _hasProvider; }
            set { _hasProvider = value; }
        }

        /// <summary>
        /// Creates the control that will display on the page
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            EnsureChildControls();


            // Get categories from the database table Category
            var categories = _dataSource.GetAllCategoriesWithProvider(_hasProvider);

            // Get category collection that is structured as a family tree
            var categorymapper = new CategoryMapper(categories);


            // Build the html to represent the control
            var html = new StringBuilder();

            
            html.Append("<ul id=\"navigation\">");

            // Create each list item <li> Category information </li>
            foreach (var child in categorymapper.Categories)
            {
                RenderCategory(html, child);
            }

            html.Append("</ul>");


            Controls.Add(new LiteralControl(html.ToString()));
        }

        /// <summary>
        /// Build the list item using the category object properties
        /// </summary>
        /// <param name="html"></param>
        /// <param name="cat"></param>
        private static void  RenderCategory(StringBuilder html, Category cat)
        {
            html.Append("<li>");

            if (cat.Depth == 1)
            {
                html.Append(cat.Description);
            }
            else
            {

                html.Append("<a href=\"" + ConfigurationManager.AppSettings["CategoryResultPage"] + "?cat=" + cat.Id + "\">");
                html.Append(cat.Description);
                html.Append("</a>");
            }

            // If the category (list item) has children, create a new nested unordered list and call itself to populate the list
            // items with the child categories (list items).
            if (cat.Categories.Count > 0)
            {
                html.Append("<ul>");
                foreach (var child in cat.Categories)
                {
                    RenderCategory(html, child);
                }
                html.Append("</ul>");
            }
            html.Append("</li>");
        }
    }
}