using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Escc.Data.Ado;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.ApplicationBlocks.ExceptionManagement;

namespace Escc.SupportWithConfidence.Controls
{
    public static class DataAccess
    {
        /// <summary>
        /// Returns datatable using stored procedure and parameter collection
        /// </summary>
        /// <returns></returns>
        private static DataSet QueryDatabase(string storedProcedureName, SqlParameter[] parameters, ConnectionType connectionType)
        {
            try
            {
                var cn = connectionType == ConnectionType.User ? new SqlConnection(ConfigurationManager.ConnectionStrings["SupportwithConfidenceUser"].ConnectionString) : new SqlConnection(ConfigurationManager.ConnectionStrings["SupportwithConfidenceAdmin"].ConnectionString);

                DataSet ds = SqlHelper.ExecuteDataset(cn, CommandType.StoredProcedure, storedProcedureName, parameters);

                return ds;
            }
            catch (SqlException ex)
            {
                ExceptionManager.Publish(ex);

                return null;
            }
        }




        private static bool SaveToDatabase(string storedProcedureName, SqlParameter[] parameters)
        {
            try
            {
                var cn  =   new SqlConnection(ConfigurationManager.ConnectionStrings["SupportwithConfidenceAdmin"].ConnectionString);
              

                SqlHelper.ExecuteNonQuery(cn, CommandType.StoredProcedure, storedProcedureName, parameters);
                return true;
               
            }
            catch (SqlException ex)
            {
                ExceptionManager.Publish(ex);
                return false;

                
            }
        }

        public static bool PostcodeCount()
        {
            return SaveToDatabase("usp_PostcodeUsage_Insert", null);
        }


        /// <summary>
        /// Return all providers or only providers who are approved
        /// </summary>
        /// <returns></returns>
        public static DataSet GetAllProviders()
        {
           

            return QueryDatabase("usp_Admin_GetAllProviders", null, ConnectionType.Admin);
        }


        /// <summary>
        /// Return all providers or only providers who are approved
        /// </summary>
        /// <param name="thatAreApproved"></param>
        /// <returns></returns>
        public static DataSet GetAllProviders(bool thatAreApproved)
        {
            var parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@IsApproved", SqlDbType.Bit) { Value = thatAreApproved };

            return QueryDatabase("usp_GetAllProviders", parameters, ConnectionType.User);
        }

        /// <summary>
        /// Return provider using provider Id or only return the provider by provider Id if approved
        /// </summary>
        /// <param name="id"></param>
        /// <param name="thatIsApproved"></param>
        /// <returns></returns>
        public static DataSet GetProviderById(int id, bool thatIsApproved)
        {
            var parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@FlareId", SqlDbType.Int) { Value = id };
            parameters[1] = new SqlParameter("@IsApproved", SqlDbType.Int) { Value = thatIsApproved };

            return QueryDatabase("usp_GetProviderById", parameters, ConnectionType.User);
        }


        public static DataSet GetAllCategories(bool thatAreApproved)
        {
            var parameters = new SqlParameter[1];

            parameters[0] = new SqlParameter("@IsApproved", SqlDbType.Int) { Value = thatAreApproved };
            return QueryDatabase("usp_GetAllCategories", parameters, ConnectionType.User);
        }

    
        public static DataSet GetAllCategoriesWithProvider(bool hasProvider)
        {
            var parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@HasProvider", SqlDbType.Int) { Value = hasProvider };
            return QueryDatabase("usp_GetAllCategoriesWithProvider", parameters, ConnectionType.User);
        }

        public static DataSet GetProviderByCategoryId(int id, bool thatIsApproved)
        {
            var parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@FlareId", SqlDbType.Int) { Value = id };
            parameters[1] = new SqlParameter("@IsApproved", SqlDbType.Int) { Value = thatIsApproved };

            return QueryDatabase("usp_GetProviderByCategoryId", parameters, ConnectionType.User);
        }
       
        public static DataSet GetPagedResultsByCategoryId(int easting, int northing, int pageindex, int pagesize, int categoryId)
        {
            var parameters = new SqlParameter[5];

            parameters[0] = new SqlParameter("@Easting", SqlDbType.Int);
            if (easting == 0) { parameters[0].Value = System.DBNull.Value; } else { parameters[0].Value = easting; }
            parameters[1] = new SqlParameter("@Northing", SqlDbType.Int) { Value = northing };
            if (northing == 0) { parameters[1].Value = System.DBNull.Value; } else { parameters[1].Value = northing; }
            parameters[2] = new SqlParameter("@PageIndex", SqlDbType.Int) { Value = pageindex };
            parameters[3] = new SqlParameter("@PageSize", SqlDbType.Int) { Value = pagesize };
            parameters[4] = new SqlParameter("@CategoryId", SqlDbType.Int) { Value = categoryId };
           
          
            return  QueryDatabase("usp_GetPagedResultsByCategoryId", parameters, ConnectionType.User);
        }

        public static DataSet GetPagedResultsForSearchTerm(int pageindex, int pagesize, int easting, int northing, string searchTerm)
        {
            var parameters = new SqlParameter[5];
            parameters[0] = new SqlParameter("@PageIndex", SqlDbType.Int) { Value = pageindex };
            parameters[1] = new SqlParameter("@PageSize", SqlDbType.Int) { Value = pagesize };
            parameters[2] = new SqlParameter("@Easting", SqlDbType.Int);
            if (easting == 0) { parameters[2].Value = System.DBNull.Value; } else { parameters[2].Value = easting; }
           parameters[3] = new SqlParameter("@Northing", SqlDbType.Int);
           if (northing == 0) { parameters[3].Value = System.DBNull.Value; } else { parameters[3].Value = northing; }
            parameters[4] = new SqlParameter("@Name", SqlDbType.VarChar) { Value = searchTerm ?? string.Empty  };
           

                        
            return QueryDatabase("usp_GetPagedResultsForSearchTerm", parameters, ConnectionType.User);
        }

        public static DataSet GetCategoryById(int id)
        {
            var parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@CategoryId", SqlDbType.Int) { Value = id };
            return QueryDatabase("usp_GetCategoryById", parameters, ConnectionType.User);
        }

        public static bool SaveProviderInformation(int id, string experience, string expertise, string background, string accreditation, string services, string costs, string crb, bool publishToWeb)
        {
            var parameters = new SqlParameter[9];
            parameters[0] = new SqlParameter("@FlareId", SqlDbType.Int) { Value = id };
            parameters[1] = new SqlParameter("@Experience", SqlDbType.Text) { Value = experience };
            parameters[2] = new SqlParameter("@Expertise", SqlDbType.Text) { Value = expertise };
            parameters[3] = new SqlParameter("@Background", SqlDbType.Text) { Value = background };
            parameters[4] = new SqlParameter("@Accreditation", SqlDbType.Text) { Value = accreditation };
            parameters[5] = new SqlParameter("@Services", SqlDbType.Text) { Value = services };
            parameters[6] = new SqlParameter("@Costs", SqlDbType.Text) { Value = costs };
            parameters[7] = new SqlParameter("@Crb", SqlDbType.Text) { Value = crb };
            parameters[8] = new SqlParameter("@PublishToWeb", SqlDbType.Bit) { Value = publishToWeb };

           return SaveToDatabase("usp_Admin_ProviderExtra_Update", parameters);


        }

        public static DataSet GetAllProvidersPaged(int pageIndex, int pageSize)
        {
            var parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@PageIndex", SqlDbType.Int) { Value = pageIndex };
            parameters[1] = new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize };
            return QueryDatabase("usp_Admin_GetAllProviders_Paged", parameters, ConnectionType.Admin);

        }


        public static DataSet GetProvider(int providerId)
        {
            var parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@FlareId", SqlDbType.Int) { Value = providerId };
            return QueryDatabase("usp_Admin_GetProvider", parameters, ConnectionType.Admin);
        }


       


        public static bool GetImage(int id)
        {
            var parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@FlareId", SqlDbType.Int) { Value = id };
          
            return SaveToDatabase("usp_Admin_ProviderExtra_Select_Photo", parameters);
        }


        /// <summary>
        /// Gets an image file from the database
        /// </summary>
        /// <param name="imageDataId">The record id for the stored image</param>
        /// <param name="includeBlobData">If false, a special stored procedure is called that gets just the file details without the actual BLOB data.
        /// If true, the normal file retrieval stored procedure is called.</param>
        /// <returns>The file data for the stored image</returns>
        public static DatabaseFileData GetImageFromDb(        
            int imageDataId,
            bool includeBlobData)
        {
            // Initialise all the output parameters
            var fileData = new DatabaseFileData();

            // Retrieve the image file from the database storage
            string storedProcedure = includeBlobData ? "usp_GetImageData" : "usp_GetFastImageDetails";
            DataTable dt = DatabaseFileStorage.GetFile(ConfigurationManager.ConnectionStrings["SupportwithConfidenceAdmin"].ConnectionString, storedProcedure, imageDataId);

            // Get the image's original filename
            fileData.FileOriginalName = FieldData.getString(dt, "FileOriginalName");

            // Get the image's content type
            fileData.FileContentType = FieldData.getString(dt, "MIMEContentType");

            // Get the image size
            fileData.FileSize = FieldData.getInteger(dt, "FileSize");

            if (includeBlobData)
            {
                // Get the image data 
                object objBlobData = FieldData.getObject(dt, "FileData");
                fileData.FileBLOBData = (byte[])objBlobData;
            }

            return fileData;
        }


        /// <summary>
        /// Saves an image file to the database
        /// </summary>
        /// <param name="imageDataId">The record id for the stored image in the database if we updating an existing one.</param>
        /// <param name="fileData"></param>
        /// <param name="modifiedBy"></param>
        /// <param name="flareId"></param>
        /// <param name="isRemoved"></param>
        /// <returns>The record id for the stored image</returns>
        public static int SaveImageToDb(
            int imageDataId,
            DatabaseFileData fileData,
            string modifiedBy,int flareId, bool isRemoved)
        {
            // Store the image file as a file data record

            if (isRemoved)
            {
                DeleteImageToDb(imageDataId);
                return 0;
            }
            int fileId = DatabaseFileStorage.SaveFile(ConfigurationManager.ConnectionStrings["SupportwithConfidenceAdmin"].ConnectionString, "usp_AddAmendImageData", imageDataId, fileData, modifiedBy);

            if (fileId > 0)
            {

                var parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@FlareId", SqlDbType.Int) { Value = flareId };
                parameters[1] = new SqlParameter("@PhotographId", SqlDbType.Int) { Value = fileId };
                parameters[2] = new SqlParameter("@Remove", SqlDbType.Bit) { Value = false };
                bool success = SaveToDatabase("usp_Admin_ProviderExtra_Update_Photo", parameters);

                if (success)
                {
                    return fileId;
                }
                return 0;
            }
            return 0;
        }


        /// <summary>
        /// Deletes an image file from the database
        /// </summary>
        /// <param name="imageDataId">The record id for the stored image in the database.</param>
              
        public static void DeleteImageToDb(
          
            int imageDataId)            
        {
            DatabaseFileStorage.DeleteFile(ConfigurationManager.ConnectionStrings["SupportwithConfidenceAdmin"].ConnectionString, "usp_DeleteImageData", imageDataId);

           
        }

        public static string DotNetProjectName
        {
            get { return ConfigurationManager.AppSettings["ProjectName"]; }
        }


    }
}