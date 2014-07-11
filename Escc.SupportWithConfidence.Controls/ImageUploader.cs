using System;
using System.Drawing;
using System.IO;
using System.Security.Principal;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using EsccWebTeam.Data.Ado;
using EsccWebTeam.DatabaseFileControls;


namespace Escc.SupportWithConfidence.Controls
{
    public class ImageUploader : WebControl, INamingContainer
    {
        private readonly HtmlImage _img = new HtmlImage();
        private readonly String _path = HttpContext.Current.Request.PhysicalApplicationPath + "images\\";
        private int _fileDataId;

        public ImageUploader()
            : base(HtmlTextWriterTag.Div)
        {
        }

        public int FileDataId
        {
            get { return _fileDataId; }
            set { _fileDataId = value; }
        }

        public int FlareId { get; set; }


        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            EnsureChildControls();

            CssClass = "imageupload";


            Uri imageUrl = MultiFileAttachmentBaseControl.GetImageUrl(_fileDataId, DataAccess.DotNetProjectName);
            if (imageUrl != null)
            {
                HttpContext.Current.Session.Add("CroppedImagePath", imageUrl);
                _img.Src = imageUrl.AbsoluteUri;
                _img.Alt = "";
                _img.Attributes["class"] = "photo";
                Controls.Add(_img);
                // ADD remove  update button
                Controls.Add(new LiteralControl("<div class=\"removephoto\">"));
                var btnRemove = new Button {Text = @"Remove"};
                Controls.Add(btnRemove);
                btnRemove.Click += btnRemove_Click;
                Controls.Add(new LiteralControl("</div>"));
            }
            else
            {
                _img.Src = "images/" + "silhouette.jpg";
                _img.Alt = @"No photo for provider";
                Controls.Add(_img);


                Controls.Add(new LiteralControl("<div class=\"uploadphoto\">"));

                Controls.Add(new FileUpload {ID = "Upload"});
                Controls.Add(new LiteralControl("<div>"));
                var btnUpload = new Button {Text = @"Add"};
                Controls.Add(btnUpload);
                btnUpload.Click += btnUpload_Click;


                Controls.Add(new LiteralControl("</div>"));
                Controls.Add(new LiteralControl("</div>"));
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session.Add("IsRemoved", true);
            HttpContext.Current.Session.Add("HasImage", true);
            var saveSuccessValidator = new CustomValidator
                {
                    Display = ValidatorDisplay.None,
                    EnableClientScript = false,
                    IsValid = false,
                    ErrorMessage = @"Click save to confirm image removal."
                };
            Controls.Add(saveSuccessValidator);
        }


        //Used by parent control to call save
        // TODO: Need to fix this still
        public void Save()
        {
           WindowsIdentity identity = WindowsIdentity.GetCurrent();
            if (identity == null) return;
            string user = identity.Name;

            if (HttpContext.Current.Session["HasImage"] == null) return;
            bool isRemoved = Convert.ToBoolean(HttpContext.Current.Session["IsRemoved"]);
            /*
           if (isRemoved)
           {
               int fileDataId = DataAccess.SaveImageToDb(FileDataId, null, user, FlareId, true);

               if (fileDataId > 0)
               {
                   //@"Record updated, the providers image was saved sucessfully.
               }
           }
           else
           {
               FileStream fs = File.OpenRead(HttpContext.Current.Session["CroppedImagePath"].ToString());

               string filename = HttpContext.Current.Session["newfilename"].ToString();

               var fileData = new DatabaseFileData(fs, filename, "Provider photo");

               int fileDataId = DataAccess.SaveImageToDb(FileDataId, fileData, user, FlareId, false);

               fs.Close();
               if (fileDataId > 0)
               {
                   //@"Record updated, the providers image was saved sucessfully.
               }
           }*/
        }


        private void btnUpload_Click(object sender, EventArgs e)
        {
            var upload = (FileUpload) FindControl("Upload");
            bool fileOk = false;
            bool fileSaved = false;
            string originalFileName = string.Empty;
            string savenewfile = string.Empty;
            string fileExtension = null;
            string newfile = string.Empty;

            if (upload.HasFile)
            {
                HttpContext.Current.Session.Add("HasImage", true);
                HttpContext.Current.Session.Add("WorkingImage", upload.FileName);
                originalFileName = upload.FileName;

                string extension = Path.GetExtension(HttpContext.Current.Session["WorkingImage"].ToString());
                if (extension != null)
                    fileExtension = extension.ToLower();

                String[] allowedExtensions = {".png", ".jpeg", ".jpg", ".gif"};

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
                try
                {
                    upload.PostedFile.SaveAs(_path + HttpContext.Current.Session["WorkingImage"]);

                    string saveoriginal = _path + originalFileName;
                    newfile = "resized" + originalFileName;
                    HttpContext.Current.Session.Add("newfilename", newfile);
                    savenewfile = String.Format("{0}resized{1}", _path, originalFileName);

                    ResizeImage(saveoriginal, savenewfile, 217, 182, true);

                    fileSaved = true;
                }

                catch (Exception)
                {
                    //   lblError.Text = "File could not be uploaded." + ex.Message.ToString();

                    //    lblError.Visible = true;

                    fileSaved = false;
                }
            }


            if (fileSaved)
            {
                _img.Src = "images/" + newfile;
                HttpContext.Current.Session.Add("CroppedImagePath", savenewfile);
                HttpContext.Current.Session["IsRemoved"] = false;
            }
        }

        public static void ResizeImage(string originalFile, string newFile, int newWidth, int maxHeight,
                                       bool onlyResizeIfWider)
        {
            System.Drawing.Image fullsizeImage = System.Drawing.Image.FromFile(originalFile);

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

            int newHeight = fullsizeImage.Height*newWidth/fullsizeImage.Width;
            if (newHeight > maxHeight)
            {
                // Resize with height instead
                newWidth = fullsizeImage.Width*maxHeight/fullsizeImage.Height;
                newHeight = maxHeight;
            }

            System.Drawing.Image newImage = fullsizeImage.GetThumbnailImage(newWidth, newHeight, null, IntPtr.Zero);

            // Clear handle to original file so that we can overwrite it if necessary
            fullsizeImage.Dispose();

            // Save resized picture
            newImage.Save(newFile);
        }
    }
}