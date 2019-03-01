using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Escc.DatabaseFileControls.WebForms;
using Escc.EastSussexGovUK.Mvc;
using Escc.SupportWithConfidence.Admin.Data;
using Escc.SupportWithConfidence.Admin.Models;
using Escc.SupportWithConfidence.Controls;
using Exceptionless;

namespace Escc.SupportWithConfidence.Admin.Controllers
{
    public class ProviderController : Controller
    {
        private EsccSupportWithConfidenceAdminContext db = new EsccSupportWithConfidenceAdminContext();

        public async Task<ActionResult> Index()
        {
            int reference;
            int.TryParse(Request.QueryString["ref"], out reference);

            var model = new ProviderViewModel();
            model.Accreditations = await LoadAllAccreditations();

            IProviderDataRepository _repository = new SqlServerProviderDataRepository();
            model.Provider = ReadProviderById(_repository, reference);
            if (model.Provider != null)
            {
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
            else return new HttpStatusCodeResult(404);
        }

        private async Task<IEnumerable<Controls.Accreditation>> LoadAllAccreditations()
        {
            var accreditationData = await db.Accreditations.ToListAsync();
            return accreditationData.Select(result => new Controls.Accreditation()
            {
                AccreditationId = result.AccreditationId,
                Name = result.Name
            });
        }

        private Provider ReadProviderById(IProviderDataRepository repository, int id)
        {
            var proMapper = new ProviderMapper(repository);
            proMapper.GetProvider(id);
            if (proMapper.Providers.Count > 0)
            {
                var provider = proMapper.Providers[0];
                provider.ImageUrl = MultiFileAttachmentBaseControl.GetImageUrl(provider.PhotographId, ConfigurationManager.AppSettings["ProjectName"]);
                return provider;
            }
            return null;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(ProviderViewModel model)
        {
            var repo = new SqlServerProviderDataRepository();
            if (ModelState.IsValid)
            {
                if (Request.Files.Count > 0)
                {
                    SaveImage(Request.Files[0]);
                }
                bool success = repo.SaveProviderInformation(model.Provider.FlareId, model.Provider.Experience, model.Provider.Expertise, model.Provider.Background,
                                                            model.Provider.Services, model.Provider.Costs, model.Provider.Crb, model.Provider.PublishToWeb);

                var selectedAccreditations = Request.Form["Provider.Accreditations.AccreditationId"]?.Split(',');
                repo.ClearAccreditations(model.Provider.FlareId);
                if (selectedAccreditations != null)
                {
                    foreach (var accreditationId in selectedAccreditations)
                    {
                        repo.SaveProviderAccreditation(model.Provider.FlareId, accreditationId);
                    }
                }

                repo.ClearCategories(model.Provider.FlareId);
                if (model.Provider.CategoryIds != null)
                {
                    foreach (var categoryId in model.Provider.CategoryIds)
                    {
                        repo.SaveProviderCategory(model.Provider.FlareId, categoryId);
                    }
                }

                if (success)
                {
                    return new RedirectResult(Url.Content("~/providers.aspx"));
                }
            }

            // Error, and the posted model doesn't have all the data so get the original and update it
            var provider = ReadProviderById(repo, model.Provider.FlareId);
            provider.Experience = model.Provider.Experience;
            provider.Expertise = model.Provider.Expertise;
            provider.Background = model.Provider.Background;
            provider.Services = model.Provider.Services;
            provider.Costs = model.Provider.Costs;
            provider.Crb = model.Provider.Crb;
            provider.PublishToWeb = model.Provider.PublishToWeb;
            model.Provider = provider;
            model.Accreditations = await LoadAllAccreditations();
            ModelState.AddModelError(string.Empty, "Save failed");

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

        private void SaveImage(HttpPostedFileBase upload)
        {
            bool fileOk = false;
            string originalFileName = string.Empty;
            string savenewfile = string.Empty;
            string fileExtension = null;
            string newfile = string.Empty;

            originalFileName = upload.FileName;

            string extension = Path.GetExtension(originalFileName);
            if (extension != null)
            {
                fileExtension = extension.ToLower();

                String[] allowedExtensions = { ".png", ".jpeg", ".jpg", ".gif" };

                for (int i = 0; i < allowedExtensions.Length; i++)
                {
                    if (fileExtension == allowedExtensions[i])
                    {
                        fileOk = true;
                    }
                }
            }

            if (fileOk)
            {
                var _path = Request.PhysicalApplicationPath + "images\\";
                var saveToPath = _path + "resized-" + Guid.NewGuid() + extension;

                var imageToSave = ResizeAndSaveImage(upload, saveToPath, 217, 182, true);

                // There was no code to save the image when converting from WebForms!
                // It's supposed to go in the database.
            }
        }

        public static Image ResizeAndSaveImage(HttpPostedFileBase upload, string newFile, int newWidth, int maxHeight,
                                       bool onlyResizeIfWider)
        {
            Image fullsizeImage = Image.FromStream(upload.InputStream);

            // Prevent using images internal thumbnail
            fullsizeImage.RotateFlip(RotateFlipType.Rotate180FlipNone);
            fullsizeImage.RotateFlip(RotateFlipType.Rotate180FlipNone);

            if (onlyResizeIfWider)
            {
                if (fullsizeImage.Width <= newWidth)
                {
                    newWidth = fullsizeImage.Width;
                }
            }

            int newHeight = fullsizeImage.Height * newWidth / fullsizeImage.Width;
            if (newHeight > maxHeight)
            {
                // Resize with height instead
                newWidth = fullsizeImage.Width * maxHeight / fullsizeImage.Height;
                newHeight = maxHeight;
            }

            return fullsizeImage.GetThumbnailImage(newWidth, newHeight, null, IntPtr.Zero);
        }
    }
}