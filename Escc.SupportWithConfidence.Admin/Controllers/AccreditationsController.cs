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

namespace Escc.SupportWithConfidence.Admin.Controllers
{
    public class AccreditationsController : Controller
    {
        private EsccSupportWithConfidenceAdminContext db = new EsccSupportWithConfidenceAdminContext();

        // GET: Accreditations
        public async Task<ActionResult> Index()
        {
            var accreditationData = await db.Accreditations.ToListAsync();
            return View(new AccreditationViewModel() { Accreditations = accreditationData.Select(result => new Controls.Accreditation()
                            {
                                AccreditationId = result.AccreditationId,
                                Name = result.Name,
                                Website = result.Website
                            })});
        }

        // GET: Accreditations/Create
        public ActionResult Create()
        {
            return View(new AccreditationViewModel());
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

            return View(new AccreditationViewModel()
            {
                Accreditation = new Controls.Accreditation()
                {
                    AccreditationId = accreditation.AccreditationId,
                    Name = accreditation.Name,
                    Website = accreditation.Website
                }
            });
        }

        // GET: Accreditations/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Accreditation accreditation = await db.Accreditations.FindAsync(id);
            if (accreditation == null)
            {
                return HttpNotFound();
            }
            return View(new AccreditationViewModel() {
                Accreditation = new Controls.Accreditation()
                {
                    AccreditationId = accreditation.AccreditationId,
                    Name = accreditation.Name,
                    Website = accreditation.Website
                }
            });
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
            return View(new AccreditationViewModel() {
                Accreditation = new Controls.Accreditation()
                {
                    AccreditationId = accreditation.AccreditationId,
                    Name = accreditation.Name,
                    Website = accreditation.Website
                }
            });
        }

        // GET: Accreditations/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Accreditation accreditation = await db.Accreditations.FindAsync(id);
            if (accreditation == null)
            {
                return HttpNotFound();
            }
            return View(new AccreditationViewModel() {
                Accreditation = new Controls.Accreditation()
                {
                    AccreditationId = accreditation.AccreditationId,
                    Name = accreditation.Name,
                    Website = accreditation.Website
                }
            });
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
