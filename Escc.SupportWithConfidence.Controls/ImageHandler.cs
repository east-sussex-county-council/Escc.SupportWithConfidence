using EsccWebTeam.Data.Ado;
using EsccWebTeam.DatabaseFileControls;

namespace Escc.SupportWithConfidence.Controls
{
    /// <summary>
    /// Enables an image to be displayed on a web page directly from database storage
    /// </summary>
    public class ImageHandler : BaseFileAttachmentHandler
    {
        /// <summary>
        /// Gets an image file from the database.
        /// </summary>
        /// <param name="fileDataId">The record id for the stored image</param>
        /// <param name="includeBlobData">If false, a special stored procedure is called that gets just the file details without the actual BLOB data.
        /// If true, the normal file retrieval stored procedure is called.</param>
        /// <returns>The file data for the stored image</returns>
        public override DatabaseFileData GetFileAttachment(int fileDataId, bool includeBlobData)
        {
            // Retrieve the image file from the database
            return DataAccess.GetImageFromDb(fileDataId, includeBlobData);
        }   

        /// <summary>
        /// Specifies the querystring parameter used to identify the file to fetch from the database.
        /// </summary>
        /// <returns></returns>
        public override string QueryStringParameterNameForFileID()
        {
            return "imageid";
        }
    }
}

