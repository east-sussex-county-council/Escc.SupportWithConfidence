using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using Dapper;
using Escc.Data.Ado;
using Exceptionless;

namespace Escc.SupportWithConfidence.Controls
{
    /// <summary>
    /// Read and write a canonical copy of data from a SQL Server database
    /// </summary>
    public class SqlServerProviderDataRepository : IProviderDataRepository
    {
        private bool SaveToDatabase(string storedProcedureName, DynamicParameters parameters)
        {
            try
            {
                using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["SupportwithConfidenceAdmin"].ConnectionString))
                {
                    cn.Execute(storedProcedureName, parameters, commandType: CommandType.StoredProcedure);
                }
                return true;
               
            }
            catch (SqlException ex)
            {
                ex.ToExceptionless().Submit();
                return false;

                
            }
        }

        public bool SaveProviderInformation(int id, string experience, string expertise, string background, string accreditation, string services, string costs, string crb, bool publishToWeb)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@FlareId", id, DbType.Int32);
            parameters.Add("@Experience", experience, DbType.AnsiString);
            parameters.Add("@Expertise", expertise, DbType.AnsiString);
            parameters.Add("@Background", background, DbType.AnsiString);
            parameters.Add("@Accreditation", accreditation, DbType.AnsiString);
            parameters.Add("@Services", services, DbType.AnsiString);
            parameters.Add("@Costs", costs, DbType.AnsiString);
            parameters.Add("@Crb", crb, DbType.AnsiString);
            parameters.Add("@PublishToWeb", publishToWeb, DbType.Boolean);

           return SaveToDatabase("usp_Admin_ProviderExtra_Update", parameters);
        }

        public bool GetImage(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@FlareId", id, DbType.Int32);
          
            return SaveToDatabase("usp_Admin_ProviderExtra_Select_Photo", parameters);
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
        public int SaveImageToDb(
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

                var parameters = new DynamicParameters();
                parameters.Add("@FlareId", flareId, DbType.Int32);
                parameters.Add("@PhotographId", fileId, DbType.Int32);
                parameters.Add("@Remove", false, DbType.Boolean);
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
              
        public void DeleteImageToDb(
          
            int imageDataId)            
        {
            DatabaseFileStorage.DeleteFile(ConfigurationManager.ConnectionStrings["SupportwithConfidenceAdmin"].ConnectionString, "usp_DeleteImageData", imageDataId);

           
        }


        /// <summary>
        /// Gets a category of providers by its identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public DataSet GetCategoryById(int id)
        {
            var parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@CategoryId", SqlDbType.Int) { Value = id };
            return SqlServerProviderDataSource.QueryDatabase("usp_GetCategoryById", parameters, ConnectionType.User);
        }

        /// <summary>
        /// Return all providers or only providers who are approved
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllProviders()
        {
            return SqlServerProviderDataSource.QueryDatabase("usp_Admin_GetAllProviders", null, ConnectionType.Admin);
        }

        /// <summary>
        /// Gets a paged subset of all providers.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public DataSet GetAllProvidersPaged(int pageIndex, int pageSize)
        {
            var parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@PageIndex", SqlDbType.Int) { Value = pageIndex };
            parameters[1] = new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize };
            return SqlServerProviderDataSource.QueryDatabase("usp_Admin_GetAllProviders_Paged", parameters, ConnectionType.Admin);

        }

        /// <summary>
        /// Gets a single provider, whether or not it is published.
        /// </summary>
        /// <param name="providerId">The provider identifier.</param>
        /// <returns></returns>
        public DataSet GetProvider(int providerId)
        {
            var parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@FlareId", SqlDbType.Int) { Value = providerId };
            return SqlServerProviderDataSource.QueryDatabase("usp_Admin_GetProvider", parameters, ConnectionType.Admin);
        }
    }
}