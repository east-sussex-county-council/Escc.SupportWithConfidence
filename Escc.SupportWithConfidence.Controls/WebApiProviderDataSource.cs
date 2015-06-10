using System;
using System.Configuration;
using System.Data;
using Escc.Data.Ado;
using EsccWebTeam.Data.Xml;

namespace Escc.SupportWithConfidence.Controls
{
    /// <summary>
    /// Get data about Support with Confidence providers from a Web API
    /// </summary>
    public class WebApiProviderDataSource : IProviderDataSource
    {
        /// <summary>
        /// Gets all categories, optionally limited to those with at least one approved provider.
        /// </summary>
        /// <param name="hasProvider">if set to <c>true</c> only select categories with at least one provider.</param>
        /// <returns></returns>
        public DataSet GetAllCategoriesWithProvider(bool hasProvider)
        {
            var api = new WebApiRequest();
            return api.Get<DataSet>(new Uri(ConfigurationManager.AppSettings["SupportWithConfidenceApiBaseUrl"] + "/Categories?hasProvider=" + hasProvider));
        }

        /// <summary>
        /// Return provider using provider Id or only return the provider by provider Id if approved
        /// </summary>
        /// <param name="id"></param>
        /// <param name="thatIsApproved"></param>
        /// <returns></returns>
        public DataSet GetProviderById(int id, bool thatIsApproved)
        {
            var api = new WebApiRequest();
            return api.Get<DataSet>(new Uri(ConfigurationManager.AppSettings["SupportWithConfidenceApiBaseUrl"] + "/Providers/" + id + "?approved=" + thatIsApproved));
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
        public DataSet GetPagedResultsByCategoryId(int easting, int northing, int pageindex, int pagesize, int categoryId)
        {
            var api = new WebApiRequest();
            return api.Get<DataSet>(new Uri(ConfigurationManager.AppSettings["SupportWithConfidenceApiBaseUrl"] + "/Providers/?easting=" + easting + "&northing=" + northing + "&page=" + pageindex + "&pagesize=" + pagesize + "&category=" + categoryId)); 
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
        public DataSet GetPagedResultsForSearchTerm(int pageindex, int pagesize, int easting, int northing, string searchTerm)
        {
            var api = new WebApiRequest();
            return api.Get<DataSet>(new Uri(ConfigurationManager.AppSettings["SupportWithConfidenceApiBaseUrl"] + "/Providers/?easting=" + easting + "&northing=" + northing + "&page=" + pageindex + "&pagesize=" + pagesize + "&search=" + searchTerm));
        }

        /// <summary>
        /// Gets an image file from the database
        /// </summary>
        /// <param name="imageDataId">The record id for the stored image</param>
        /// <param name="includeBlobData">If false, a special stored procedure is called that gets just the file details without the actual BLOB data.
        /// If true, the normal file retrieval stored procedure is called.</param>
        /// <returns>
        /// The file data for the stored image
        /// </returns>
        public DatabaseFileData GetImageFromDb(int imageDataId, bool includeBlobData)
        {
            var api = new WebApiRequest();
            return api.Get<DatabaseFileData>(new Uri(ConfigurationManager.AppSettings["SupportWithConfidenceApiBaseUrl"] + "/Images/" + imageDataId + "?includeBlobData=" + includeBlobData));
        }
    }
}