using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Escc.EastSussexGovUK.Mvc;
using data = Escc.SupportWithConfidence.Admin.Data;
using Escc.SupportWithConfidence.Admin.Models;
using Escc.SupportWithConfidence.Controls;
using Exceptionless;
using Escc.Html;
using HtmlAgilityPack;

namespace Escc.SupportWithConfidence.Admin.Controllers
{
    public class CategoriesController : Controller
    {
        private data.EsccSupportWithConfidenceAdminContext db = new data.EsccSupportWithConfidenceAdminContext();

        public async Task<ActionResult> AutoComplete(string term)
        {
            IProviderDataSource _dataSource = new SqlServerProviderDataSource();

            // Get categories from the database table Category
            var categories = await _dataSource.GetAllCategoriesWithProvider(false);

            return Json(categories.Where(x => x.Depth == 2 && x.Description.StartsWith(term, StringComparison.CurrentCultureIgnoreCase)).Select(x => new { id = x.CategoryId, value = x.Description }), JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Index()
        {
            IProviderDataSource _dataSource = new SqlServerProviderDataSource();

            // Get categories from the database table Category
            var categories = await _dataSource.GetAllCategoriesWithProvider(false);

            // Get category collection that is structured as a family tree
            var categorymapper = new CategoryMapper(categories);

            var model = new CategoriesViewModel() { Categories = categorymapper.Categories };
            var templateRequest = new EastSussexGovUKTemplateRequest(Request);
            try
            {
                model.WebChat = await templateRequest.RequestWebChatSettingsAsync();
            }
            catch (Exception ex)
            {
                // Catch and report exceptions - don't throw them and cause the page to fail
                ex.ToExceptionless().Submit();
            }
            try
            {
                model.TemplateHtml = await templateRequest.RequestTemplateHtmlAsync();
            }
            catch (Exception ex)
            {
                // Catch and report exceptions - don't throw them and cause the page to fail
                ex.ToExceptionless().Submit();
            }

            return View(model);
        }


        // GET: Categories/Create
        public async Task<ActionResult> Create()
        {
            var model = new CategoryViewModel();
            model.PossibleParentCategories = await db.Categories.Where(x => x.Depth == 1).OrderBy(x => x.Sequence).Select(x => new Category { CategoryId = x.CategoryId, Description = x.Description }).ToListAsync();

            var templateRequest = new EastSussexGovUKTemplateRequest(Request);
            try
            {
                model.WebChat = await templateRequest.RequestWebChatSettingsAsync();
            }
            catch (Exception ex)
            {
                // Catch and report exceptions - don't throw them and cause the page to fail
                ex.ToExceptionless().Submit();
            }
            try
            {
                model.TemplateHtml = await templateRequest.RequestTemplateHtmlAsync();
            }
            catch (Exception ex)
            {
                // Catch and report exceptions - don't throw them and cause the page to fail
                ex.ToExceptionless().Submit();
            }

            return View(model);
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var dataCategory = new data.Category
                {
                    Depth = model.ParentId.HasValue ? 2 : 1,
                    Description = model.Description,
                    IsActive = false,
                    ParentId = model.ParentId,
                    Sequence = model.Sequence,
                    Summary = FilterHtml(model.Summary)
                };

                db.Categories.Add(dataCategory);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            model.PossibleParentCategories = await db.Categories.Where(x => x.Depth == 1).OrderBy(x => x.Sequence).Select(x => new Category { CategoryId = x.CategoryId, Description = x.Description }).ToListAsync();

            var templateRequest = new EastSussexGovUKTemplateRequest(Request);
            try
            {
                model.WebChat = await templateRequest.RequestWebChatSettingsAsync();
            }
            catch (Exception ex)
            {
                // Catch and report exceptions - don't throw them and cause the page to fail
                ex.ToExceptionless().Submit();
            }
            try
            {
                model.TemplateHtml = await templateRequest.RequestTemplateHtmlAsync();
            }
            catch (Exception ex)
            {
                // Catch and report exceptions - don't throw them and cause the page to fail
                ex.ToExceptionless().Submit();
            }

            return View(model);
        }

        private static string FilterHtml(string html)
        {
            if (!String.IsNullOrEmpty(html))
            {
                var htmlSanitiser = new HtmlTagSanitiser();
                html = htmlSanitiser.StripTags(html, new string[] { "p", "a", "strong" });

                var htmlAgility = new HtmlDocument();
                htmlAgility.LoadHtml(html);
                var links = htmlAgility.DocumentNode.SelectNodes("//a");
                if (links != null)
                {
                    foreach (var link in links)
                    {
                        var unwanted = link.Attributes.Where(attr => attr.Name != "href").ToList();
                        foreach (var attr in unwanted)
                        {
                            link.Attributes.Remove(attr);
                        }
                    }
                }

                html = htmlAgility.DocumentNode.OuterHtml;
            }

            return html;
        }

        // GET: Categories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = new CategoryViewModel();
            var dataCategory = await db.Categories.FindAsync(id);
            if (dataCategory == null)
            {
                return HttpNotFound();
            }

            model.CategoryId = dataCategory.CategoryId;
            model.Sequence = dataCategory.Sequence;
            model.Description = dataCategory.Description;
            model.Summary = dataCategory.Summary;
            model.ParentId = dataCategory.ParentId.GetValueOrDefault();
            model.Depth = dataCategory.Depth;
            model.PossibleParentCategories = await db.Categories.Where(x => x.Depth == 1 && x.CategoryId != id).OrderBy(x => x.Sequence).Select(x => new Category { CategoryId = x.CategoryId, Description = x.Description }).ToListAsync();

            var templateRequest = new EastSussexGovUKTemplateRequest(Request);
            try
            {
                model.WebChat = await templateRequest.RequestWebChatSettingsAsync();
            }
            catch (Exception ex)
            {
                // Catch and report exceptions - don't throw them and cause the page to fail
                ex.ToExceptionless().Submit();
            }
            try
            {
                model.TemplateHtml = await templateRequest.RequestTemplateHtmlAsync();
            }
            catch (Exception ex)
            {
                // Catch and report exceptions - don't throw them and cause the page to fail
                ex.ToExceptionless().Submit();
            }

            return View(model);
        }


        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var dataCategory = new data.Category
                {
                    CategoryId = model.CategoryId,
                    Depth = model.ParentId.HasValue ? 2 : 1,
                    Description = model.Description,
                    ParentId = model.ParentId,
                    Sequence = model.Sequence,
                    Summary = FilterHtml(model.Summary)
                };

                var providerData = await new SqlServerProviderDataSource().GetPagedResultsByCategoryId(0,0,1,1,model.CategoryId);
                dataCategory.IsActive = dataCategory.Depth == 2 && providerData != null && providerData.Tables.Count > 0 && providerData.Tables[0].Rows.Count > 0;

                db.Entry(dataCategory).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            model.PossibleParentCategories = await db.Categories.Where(x => x.Depth == 1 && x.CategoryId != model.CategoryId).OrderBy(x => x.Sequence).Select(x => new Category { CategoryId = x.CategoryId, Description = x.Description }).ToListAsync();

            var templateRequest = new EastSussexGovUKTemplateRequest(Request);
            try
            {
                model.WebChat = await templateRequest.RequestWebChatSettingsAsync();
            }
            catch (Exception ex)
            {
                // Catch and report exceptions - don't throw them and cause the page to fail
                ex.ToExceptionless().Submit();
            }
            try
            {
                model.TemplateHtml = await templateRequest.RequestTemplateHtmlAsync();
            }
            catch (Exception ex)
            {
                // Catch and report exceptions - don't throw them and cause the page to fail
                ex.ToExceptionless().Submit();
            }

            return View(model);
        }


        // GET: Categories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = new CategoryViewModel();
            var dataCategory = await db.Categories.FindAsync(id);
            
            if (dataCategory == null)
            {
                return HttpNotFound();
            }

            model.CategoryId = dataCategory.CategoryId;
            model.Sequence = dataCategory.Sequence;
            model.Description = dataCategory.Description;
            model.Summary = dataCategory.Summary;
            model.ParentId = dataCategory.ParentId.GetValueOrDefault();
            model.Depth = dataCategory.Depth;

            var templateRequest = new EastSussexGovUKTemplateRequest(Request);
            try
            {
                model.WebChat = await templateRequest.RequestWebChatSettingsAsync();
            }
            catch (Exception ex)
            {
                // Catch and report exceptions - don't throw them and cause the page to fail
                ex.ToExceptionless().Submit();
            }
            try
            {
                model.TemplateHtml = await templateRequest.RequestTemplateHtmlAsync();
            }
            catch (Exception ex)
            {
                // Catch and report exceptions - don't throw them and cause the page to fail
                ex.ToExceptionless().Submit();
            }

            return View(model);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            data.Category category = await db.Categories.FindAsync(id);
            db.Categories.Remove(category);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}