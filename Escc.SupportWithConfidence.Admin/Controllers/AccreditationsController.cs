using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Escc.SupportWithConfidence.Admin.Data;
using Escc.SupportWithConfidence.Admin.Models;
using Escc.EastSussexGovUK.Mvc;
using Exceptionless;

namespace Escc.SupportWithConfidence.Admin.Controllers
{
    public class AccreditationsController : Controller
    {
        private EsccSupportWithConfidenceAdminContext db = new EsccSupportWithConfidenceAdminContext();

        // GET: Accreditations
        public async Task<ActionResult> Index()
        {
            var model = new AccreditationViewModel();
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

            var accreditationData = await db.Accreditations.ToListAsync();
            model.Accreditations = accreditationData.Select(result => new Controls.Accreditation()
            {
                AccreditationId = result.AccreditationId,
                Name = result.Name,
                Website = result.Website
            });
            return View(model);
        }

        // GET: Accreditations/Create
        public async Task<ActionResult> Create()
        {
            var model = new AccreditationViewModel();
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

        // POST: Accreditations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "AccreditationId,Name,Website")] Accreditation accreditation)
        {
            if (ModelState.IsValid)
            {
                db.Accreditations.Add(accreditation);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            var model = new AccreditationViewModel()
            {
                Accreditation = new Controls.Accreditation()
                {
                    AccreditationId = accreditation.AccreditationId,
                    Name = accreditation.Name,
                    Website = accreditation.Website
                }
            };
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

        // GET: Accreditations/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = new AccreditationViewModel();
            Accreditation accreditation = await db.Accreditations.FindAsync(id);
            if (accreditation == null)
            {
                return HttpNotFound();
            }

            model.Accreditation = new Controls.Accreditation()
            {
                AccreditationId = accreditation.AccreditationId,
                Name = accreditation.Name,
                Website = accreditation.Website
            };
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

        // POST: Accreditations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "AccreditationId,Name,Website")] Accreditation accreditation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(accreditation).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            var model = new AccreditationViewModel()
            {
                Accreditation = new Controls.Accreditation()
                {
                    AccreditationId = accreditation.AccreditationId,
                    Name = accreditation.Name,
                    Website = accreditation.Website
                }
            };
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

        // GET: Accreditations/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = new AccreditationViewModel();
            Accreditation accreditation = await db.Accreditations.FindAsync(id);
            if (accreditation == null)
            {
                return HttpNotFound();
            }

            model.Accreditation = new Controls.Accreditation()
            {
                AccreditationId = accreditation.AccreditationId,
                Name = accreditation.Name,
                Website = accreditation.Website
            };
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

        // POST: Accreditations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Accreditation accreditation = await db.Accreditations.FindAsync(id);
            db.Accreditations.Remove(accreditation);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
