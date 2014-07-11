using System;
using System.Configuration;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Escc.SupportWithConfidence.Controls
{
    /// <summary>
    /// This class allows admin users to change properties of a category such as the name, sequence and whether it's active
    /// </summary>
    public class EditCategoryControl : WebControl , INamingContainer
    {
        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            EnsureChildControls();

            var categoryId = Convert.ToInt32(HttpContext.Current.Request.QueryString["cat"]);

            // Gets the Category as a strongly typed opbject
            var categories = new CategoryMapper(DataAccess.GetCategoryById(categoryId)).Categories;

            var count = 0;
            foreach (var parentCategory in categories)
            {
                foreach (var childCategory in parentCategory.Categories)
                {
                    if (parentCategory.Id != categoryId && childCategory.Id != categoryId) continue;
                    while (count < 1)
                    {
                        var heading = new LiteralControl("<h1>" + childCategory.Description + "</h1>");
                        var formBoxOpen = new LiteralControl("<div class=\"formBox\">");
                        var formPartOpenId = new LiteralControl("<div class=\"formPart\">");
                        var lblId = new Label { ID = "lblId", Text = @"Id:", CssClass = "formLabel" };
                        var lblCategoryId = new Label { ID = "lblCategoryId", CssClass = "formControl", Text = childCategory.Id.ToString(CultureInfo.InvariantCulture) };
                        var formPartCloseId = new LiteralControl("</div>");
                        var formPartOpenCode = new LiteralControl("<div class=\"formPart\">");
                        var lblCodeId = new Label { ID = "lblCodeId", CssClass = "formLabel", Text = @"Code:" };
                        var lblCode = new Label { ID = "lblCode", CssClass = "formControl", Text = childCategory.Code };
                        var formPartCloseCode = new LiteralControl("</div>");
                        var formPartOpenDescription = new LiteralControl("<div class=\"formPart\">");
                        var lblDescription = new Label { ID = "lblDescription", AssociatedControlID = "txbDescription", Text = @"Description", CssClass = "formLabel"};
                        var txbDescription = new TextBox { ID = "txbDescription", Text = childCategory.Description, CssClass = "formControl" };
                        var formPartCloseDescription = new LiteralControl("</div>");
                        var formPartOpenDepth = new LiteralControl("<div class=\"formPart\">");
                        var lblDepthTitle = new Label { ID = "lblDepthTitle", CssClass = "formLabel", Text = @"Depth:" };
                        var lblDepth = new Label { ID = "lblDepth", CssClass = "formControl", Text = childCategory.Depth.ToString(CultureInfo.InvariantCulture) };
                        var formPartCloseDepth = new LiteralControl("</div>");
                        var formPartOpenSequence = new LiteralControl("<div class=\"formPart\">");
                        var lblSequenceTitle = new Label { ID = "lblSequenceTitle", CssClass = "formLabel", Text = @"Sequence:" };
                        var lblSequence = new Label { ID = "lblSequence", CssClass = "formControl", Text = childCategory.Sequence.ToString(CultureInfo.InvariantCulture) };
                        var formPartCloseSequence = new LiteralControl("</div>");
                        var formPartOpenActive = new LiteralControl("<div class=\"formPart\">");
                        var lblActive = new Label { ID = "lblActive", AssociatedControlID = "cbxActive", CssClass = "formLabel", Text = @"Active:" };
                        var cbxActive = new CheckBox
                            {
                                ID = "cbxActive",
                                CssClass = "formControl",
                                Checked = childCategory.IsActive
                            };
                        var formPartCloseActive = new LiteralControl("</div>");
                        var formPartOpenButton = new LiteralControl("<div class=\"formButtons\">");

                        var categorylist = new StringBuilder();

                        categorylist.Append("<h2>Associated categories</h2><ul>");

                        foreach (var child in parentCategory.Categories)
                        {
                            if (child.Id == parentCategory.Id) continue;
                            categorylist.Append(String.Format("<li><a href=\"{0}?cat={1}\">{2}</a></li>", ConfigurationManager.AppSettings["CategoryResultPage"], child.Id.ToString(CultureInfo.InvariantCulture), child.Description));
                        }

                        categorylist.Append("</ul>");

                            
                        var children = new LiteralControl(categorylist.ToString());
                        var formPartCloseButton = new LiteralControl("</div>");                          
                        var formBoxClose = new LiteralControl("</div>");


                        Controls.Add(heading);
                        Controls.Add(formBoxOpen);
                        Controls.Add(formPartOpenId);
                        Controls.Add(lblId);
                        Controls.Add(lblCategoryId);
                        Controls.Add(formPartCloseId);
                        Controls.Add(formPartOpenCode);
                        Controls.Add(lblCodeId);
                        Controls.Add(lblCode);
                        Controls.Add(formPartCloseCode);
                        Controls.Add(formPartOpenDescription);
                        Controls.Add(lblDescription);
                        Controls.Add(txbDescription);
                        Controls.Add(formPartCloseDescription);
                        Controls.Add(formPartOpenDepth);
                        Controls.Add(lblDepthTitle);
                        Controls.Add(lblDepth);
                        Controls.Add(formPartCloseDepth);
                        Controls.Add(formPartOpenSequence);
                        Controls.Add(lblSequenceTitle);
                        Controls.Add(lblSequence);
                        Controls.Add(formPartCloseSequence);
                        Controls.Add(formPartOpenActive);
                        Controls.Add(lblActive);
                        Controls.Add(cbxActive);
                        Controls.Add(formPartCloseActive);
                         
                        Controls.Add(formBoxClose);

                         
                        var btnSave = new Button { ID = "btnSave", CssClass = "button", Text = @"Save" };
                        Controls.Add(formPartOpenButton);

                        Controls.Add(btnSave);   
                        btnSave.Click += btnSave_Click;
                        Controls.Add(formPartCloseButton);
                         
                        Controls.Add(children);
                        count++;
                    }
                }
            }

        }

        void btnSave_Click(object sender, EventArgs e)
        {
            Controls.Add(new LiteralControl("<script>alert(\"Button clicked\");</script>"));
        }
    }
}
