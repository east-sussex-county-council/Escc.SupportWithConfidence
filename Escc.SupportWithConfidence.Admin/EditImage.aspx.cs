using System;
using System.Web;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing.Drawing2D;
using Escc.SupportWithConfidence.Controls;


namespace Escc.SupportWithConfidence.Admin
{
    public partial class EditImage : System.Web.UI.Page
    {
        String path = HttpContext.Current.Request.PhysicalApplicationPath + "images\\";
        

        protected void Page_Load(object sender, EventArgs e)
        {
            // Load image if in database
            // Set the cropping panel
            // After crop
            // Maybe resize before saving to database
            // Redirect user to details to view

         
           

        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {

            Boolean FileOK = false;
            Boolean FileSaved = false;
            string originalFileName = string.Empty;
            string saveoriginal = string.Empty;
            string savenewfile = string.Empty;
            string FileExtension = string.Empty;
            string newfile = string.Empty;

            if (Upload.HasFile)
            {
                Session["WorkingImage"] = Upload.FileName;
                originalFileName = Upload.FileName;

                 FileExtension = Path.GetExtension(Session["WorkingImage"].ToString()).ToLower();

                String[] allowedExtensions = { ".png", ".jpeg", ".jpg", ".gif" };

                for (int i = 0; i < allowedExtensions.Length; i++)
                {

                    if (FileExtension == allowedExtensions[i])
                    {

                        FileOK = true;

                    }

                }

            }



            if (FileOK)
            {

                try
                {

                    Upload.PostedFile.SaveAs(path + Session["WorkingImage"]);

                    saveoriginal = path + originalFileName;
                    newfile = "resized" + originalFileName;
                    Session["newfilename"] = newfile;
                    savenewfile = path + "resized" + originalFileName;

                    ResizeImage(saveoriginal, savenewfile, 217, 182, true); 

                    FileSaved = true;

                }

                catch (Exception ex)
                {

                    lblError.Text = "File could not be uploaded." + ex.Message.ToString();

                    lblError.Visible = true;

                    FileSaved = false;

                }

            }

            else
            {

                lblError.Text = "Cannot accept files of this type.";

                lblError.Visible = true;

            }



            if (FileSaved)
            {

                pnlUpload.Visible = false;

                pnlCropped.Visible = true;

                //imgCrop.ImageUrl = "images/" + Session["WorkingImage"].ToString();
                imgCrop.ImageUrl = "images/" + newfile;
                Session["CroppedImagePath"] = savenewfile;
            }

        }


        protected void btnCrop_Click(object sender, EventArgs e)
        {

            string ImageName = Session["WorkingImage"].ToString();

            int w = Convert.ToInt32(W.Value);

            int h = Convert.ToInt32(H.Value);

            int x = Convert.ToInt32(X.Value);

            int y = Convert.ToInt32(Y.Value);



            byte[] CropImage = Crop(path + ImageName, w, h, x, y);

            

            using (MemoryStream ms = new MemoryStream(CropImage, 0, CropImage.Length))
            {

                ms.Write(CropImage, 0, CropImage.Length);

                

                using (System.Drawing.Image CroppedImage = System.Drawing.Image.FromStream(ms, true))
                {

                    string SaveTo = path + "crop" + ImageName;
                    Session["CroppedImagePath"] = SaveTo;
                    CroppedImage.Save(SaveTo, CroppedImage.RawFormat);

                    pnlCrop.Visible = false;

                    pnlCropped.Visible = true;

                    imgCropped.ImageUrl = "images/crop" + ImageName;

                    
                }

            }

        }


        static byte[] Crop(string Img, int Width, int Height, int X, int Y)
        {

            try
            {

                using (System.Drawing.Image OriginalImage = System.Drawing.Image.FromFile(Img))
                {

                    using (System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(Width, Height))
                    {

                        bmp.SetResolution(OriginalImage.HorizontalResolution, OriginalImage.VerticalResolution);

                        using (System.Drawing.Graphics Graphic = System.Drawing.Graphics.FromImage(bmp))
                        {

                            Graphic.SmoothingMode = SmoothingMode.AntiAlias;

                            Graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;

                            Graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;

                            Graphic.DrawImage(OriginalImage, new System.Drawing.Rectangle(0, 0, Width, Height), X, Y, Width, Height, System.Drawing.GraphicsUnit.Pixel);

                            MemoryStream ms = new MemoryStream();

                            

                            bmp.Save(ms, OriginalImage.RawFormat);

                            return ms.GetBuffer();

                        }

                    }

                }

            }

            catch (Exception Ex)
            {

                throw (Ex);

            }

        }


        protected void btnSaveToDB_Click(object sender, EventArgs e)
        {

           

//          //  //Read in cropped image
//            FileStream fs = File.OpenRead(Session["CroppedImagePath"].ToString());
//
//            string filename = Session["newfilename"].ToString();
//
//            DatabaseFileData fileData = new DatabaseFileData(fs, filename, "alt text");
//          
//
//           int fileDataId = SqlServerProviderRepository.SaveImageToDb(0, fileData, "me", "");
//
//           fs.Close();
//           if (fileDataId > 0)
//           {
//
//
//               CustomValidator saveSuccessValidator = new CustomValidator();
//               saveSuccessValidator.Display = ValidatorDisplay.None;
//               saveSuccessValidator.EnableClientScript = false;
//               saveSuccessValidator.IsValid = false;
//               saveSuccessValidator.ErrorMessage = "Record updated, the providers image was saved sucessfully.";
//
//
//
//               validationSummary.Controls.Add(saveSuccessValidator);




           //}


        }



        public void ResizeImage(string OriginalFile, string NewFile, int NewWidth, int MaxHeight, bool OnlyResizeIfWider)
        {
            System.Drawing.Image FullsizeImage = System.Drawing.Image.FromFile(OriginalFile);

            // Prevent using images internal thumbnail
            FullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
            FullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);

            if (OnlyResizeIfWider)
            {
                if (FullsizeImage.Width <= NewWidth)
                {
                    NewWidth = FullsizeImage.Width;
                }
            }

            int NewHeight = FullsizeImage.Height * NewWidth / FullsizeImage.Width;
            if (NewHeight > MaxHeight)
            {
                // Resize with height instead
                NewWidth = FullsizeImage.Width * MaxHeight / FullsizeImage.Height;
                NewHeight = MaxHeight;
            }

            System.Drawing.Image NewImage = FullsizeImage.GetThumbnailImage(NewWidth, NewHeight, null, IntPtr.Zero);

            // Clear handle to original file so that we can overwrite it if necessary
            FullsizeImage.Dispose();

            // Save resized picture
            NewImage.Save(NewFile);
        }

        



    }
}