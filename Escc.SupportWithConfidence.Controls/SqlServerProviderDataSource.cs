using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Escc.Data.Ado;
using Exceptionless;

namespace Escc.SupportWithConfidence.Controls
{
    /// <summary>
    /// Read data about Support with Confidence approved providers from SQL Server
    /// </summary>
    public class SqlServerProviderDataSource : IProviderDataSource
    {
        /// <summary>
        /// Returns datatable using stored procedure and parameter collection
        /// </summary>
        /// <returns></returns>
        internal static DataSet QueryDatabase(string storedProcedureName, SqlParameter[] parameters, ConnectionType connectionType)
        {
            try
            {
                using (var cn = (connectionType == ConnectionType.User) ? new SqlConnection(ConfigurationManager.ConnectionStrings["SupportwithConfidenceUser"].ConnectionString) : new SqlConnection(ConfigurationManager.ConnectionStrings["SupportwithConfidenceAdmin"].ConnectionString))
                {
                    using (var command = new SqlCommand(storedProcedureName, cn))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(parameters);

                        using (var adapter = new SqlDataAdapter(command))
                        {
                            using (var ds = new DataSet())
                            {
                                ds.Locale = CultureInfo.CurrentCulture;
                                adapter.Fill(ds);

                                return ds;
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                ex.ToExceptionless().Submit();

                return null;
            }
        }


        /// <summary>
        /// Gets all categories, optionally limited to those with at least one approved provider.
        /// </summary>
        /// <param name="hasProvider">if set to <c>true</c> only select categories with at least one provider.</param>
        /// <returns></returns>
        public Task<IEnumerable<Category>> GetAllCategoriesWithProvider(bool hasProvider)
        {
            var parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@HasProvider", SqlDbType.Int) { Value = hasProvider };
            var dataSet = QueryDatabase("usp_GetAllCategoriesWithProvider", parameters, ConnectionType.User);
            if (dataSet == null) return null;

            var categories = new List<Category>();

            foreach (DataRow dbcategory in dataSet.Tables[0].Rows)
            {
                categories.Add(new Category
                {
                    CategoryId = Convert.ToInt16(dbcategory["CategoryId"]),
                    Description = dbcategory["Description"].ToString(),
                    ParentId =
                            dbcategory["ParentId"] == DBNull.Value
                                ? 0
                                : Convert.ToInt16(dbcategory["ParentId"]),
                    Depth = Convert.ToInt16(dbcategory["Depth"]),
                    IsActive = Convert.ToBoolean(dbcategory["IsActive"]),
                    Sequence = Convert.ToInt32(dbcategory["Sequence"])
                }
                );
            }

            return Task.FromResult(categories as IEnumerable<Category>);
        }

        /// <summary>
        /// Return provider using provider Id or only return the provider by provider Id if approved
        /// </summary>
        /// <param name="id"></param>
        /// <param name="thatIsApproved"></param>
        /// <returns></returns>
        public Task<DataSet> GetProviderById(int id, bool thatIsApproved)
        {
            var parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@FlareId", SqlDbType.Int) { Value = id };
            parameters[1] = new SqlParameter("@IsApproved", SqlDbType.Int) { Value = thatIsApproved };

            return Task.FromResult(QueryDatabase("usp_GetProviderById", parameters, ConnectionType.User));
        }

        /// <summary>
        /// Gets a page of approved providers matching a category identifier.
        /// </summary>
        /// <param name="easting">The easting.</param>
        /// <param name="northing">The northing.</param>
        /// <param name="pageindex">The pageindex.</param>
        /// <param name="pagesize">The pagesize.</param>
        /// <param name="categoryId">The category identifier.</param>
        /// <returns></returns>
        public Task<DataSet> GetPagedResultsByCategoryId(int easting, int northing, int pageindex, int pagesize, int categoryId)
        {
            var parameters = new SqlParameter[5];

            parameters[0] = new SqlParameter("@Easting", SqlDbType.Int);
            if (easting == 0) { parameters[0].Value = System.DBNull.Value; } else { parameters[0].Value = easting; }
            parameters[1] = new SqlParameter("@Northing", SqlDbType.Int) { Value = northing };
            if (northing == 0) { parameters[1].Value = System.DBNull.Value; } else { parameters[1].Value = northing; }
            parameters[2] = new SqlParameter("@PageIndex", SqlDbType.Int) { Value = pageindex };
            parameters[3] = new SqlParameter("@PageSize", SqlDbType.Int) { Value = pagesize };
            parameters[4] = new SqlParameter("@CategoryId", SqlDbType.Int) { Value = categoryId };


            return Task.FromResult(QueryDatabase("usp_GetPagedResultsByCategoryId", parameters, ConnectionType.User));
        }

        /// <summary>
        /// Gets a page of approved providers matching a search term.
        /// </summary>
        /// <param name="pageindex">The pageindex.</param>
        /// <param name="pagesize">The pagesize.</param>
        /// <param name="easting">The easting.</param>
        /// <param name="northing">The northing.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <returns></returns>
        public Task<DataSet> GetPagedResultsForSearchTerm(int pageindex, int pagesize, int easting, int northing, string searchTerm)
        {
            var parameters = new SqlParameter[5];
            parameters[0] = new SqlParameter("@PageIndex", SqlDbType.Int) { Value = pageindex };
            parameters[1] = new SqlParameter("@PageSize", SqlDbType.Int) { Value = pagesize };
            parameters[2] = new SqlParameter("@Easting", SqlDbType.Int);
            if (easting == 0) { parameters[2].Value = System.DBNull.Value; } else { parameters[2].Value = easting; }
            parameters[3] = new SqlParameter("@Northing", SqlDbType.Int);
            if (northing == 0) { parameters[3].Value = System.DBNull.Value; } else { parameters[3].Value = northing; }
            parameters[4] = new SqlParameter("@Name", SqlDbType.VarChar) { Value = searchTerm ?? string.Empty };

            return Task.FromResult(QueryDatabase("usp_GetPagedResultsForSearchTerm", parameters, ConnectionType.User));
        }

        /// <summary>
        /// Return all providers or only providers who are approved
        /// </summary>
        /// <param name="thatAreApproved"></param>
        /// <returns></returns>
        public DataSet GetAllProviders(bool thatAreApproved)
        {
            var parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@IsApproved", SqlDbType.Bit) { Value = thatAreApproved };

            return QueryDatabase("usp_GetAllProviders", parameters, ConnectionType.User);
        }

        public DataSet GetAllCategories(bool thatAreApproved)
        {
            var parameters = new SqlParameter[1];

            parameters[0] = new SqlParameter("@IsApproved", SqlDbType.Int) { Value = thatAreApproved };
            return QueryDatabase("usp_GetAllCategories", parameters, ConnectionType.User);
        }

        /// <summary>
        /// Gets an image file from the database
        /// </summary>
        /// <param name="imageDataId">The record id for the stored image</param>
        /// <param name="includeBlobData">If false, a special stored procedure is called that gets just the file details without the actual BLOB data.
        /// If true, the normal file retrieval stored procedure is called.</param>
        /// <returns>The file data for the stored image</returns>
        public Task<DatabaseFileData> GetImageFromDb(
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

            return Task.FromResult(fileData);
        }
    }
}
