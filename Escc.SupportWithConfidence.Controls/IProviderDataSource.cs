using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Escc.Data.Ado;

namespace Escc.SupportWithConfidence.Controls
{
    /// <summary>
    /// A source for reading data about Support with Confidence approved providers
    /// </summary>
    public interface IProviderDataSource
    {
        /// <summary>
        /// Gets all categories, optionally limited to those with at least one approved provider.
        /// </summary>
        /// <param name="hasProvider">if set to <c>true</c> only select categories with at least one provider.</param>
        /// <returns></returns>
        Task<IEnumerable<Category>> GetAllCategoriesWithProvider(bool hasProvider);

        /// <summary>
        /// Return provider using provider Id or only return the provider by provider Id if approved
        /// </summary>
        /// <param name="id"></param>
        /// <param name="thatIsApproved"></param>
        /// <returns></returns>
        Task<DataSet> GetProviderById(int id, bool thatIsApproved);

        /// <summary>
        /// Gets a page of approved providers matching a category identifier.
        /// </summary>
        /// <param name="easting">The easting.</param>
        /// <param name="northing">The northing.</param>
        /// <param name="pageindex">The pageindex.</param>
        /// <param name="pagesize">The pagesize.</param>
        /// <param name="categoryId">The category identifier.</param>
        /// <returns></returns>
        Task<DataSet> GetPagedResultsByCategoryId(int easting, int northing, int pageindex, int pagesize, int categoryId);

        /// <summary>
        /// Gets a page of approved providers matching a search term.
        /// </summary>
        /// <param name="pageindex">The pageindex.</param>
        /// <param name="pagesize">The pagesize.</param>
        /// <param name="easting">The easting.</param>
        /// <param name="northing">The northing.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <returns></returns>
        Task<DataSet> GetPagedResultsForSearchTerm(int pageindex, int pagesize, int easting, int northing, string searchTerm);

        /// <summary>
        /// Gets an image file from the database
        /// </summary>
        /// <param name="imageDataId">The record id for the stored image</param>
        /// <param name="includeBlobData">If false, a special stored procedure is called that gets just the file details without the actual BLOB data.
        /// If true, the normal file retrieval stored procedure is called.</param>
        /// <returns>The file data for the stored image</returns>
        Task<DatabaseFileData> GetImageFromDb(int imageDataId, bool includeBlobData);
    }
}
