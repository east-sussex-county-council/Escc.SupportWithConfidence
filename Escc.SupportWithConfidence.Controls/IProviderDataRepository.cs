using System.Data;
using Escc.Data.Ado;

namespace Escc.SupportWithConfidence.Controls
{
    public interface IProviderDataRepository
    {
        /// <summary>
        /// Return all providers 
        /// </summary>
        /// <returns></returns>
        DataSet GetAllProviders();

        /// <summary>
        /// Gets a paged subset of all providers.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        DataSet GetAllProvidersPaged(int pageIndex, int pageSize);

        /// <summary>
        /// Gets a single provider, whether or not it is published.
        /// </summary>
        /// <param name="providerId">The provider identifier.</param>
        /// <returns></returns>
        DataSet GetProvider(int providerId);

        /// <summary>
        /// Gets a category of providers by its identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        DataSet GetCategoryById(int id);


        bool SaveProviderInformation(int id, string experience, string expertise, string background, string services, string costs, string crb, bool publishToWeb);

        /// <summary>
        /// Clears the accreditations for a provider, ready to supply an updated set.
        /// </summary>
        /// <param name="flareId">The flare identifier.</param>
        void ClearAccreditations(int flareId);

        /// <summary>
        /// Adds an accreditation to a provider
        /// </summary>
        /// <param name="flareId">The flare identifier.</param>
        /// <param name="accreditationId">The accreditation identifier.</param>
        void SaveProviderAccreditation(int flareId, string accreditationId);

        bool GetImage(int id);

        /// <summary>
        /// Saves an image file to the database
        /// </summary>
        /// <param name="imageDataId">The record id for the stored image in the database if we updating an existing one.</param>
        /// <param name="fileData"></param>
        /// <param name="modifiedBy"></param>
        /// <param name="flareId"></param>
        /// <param name="isRemoved"></param>
        /// <returns>The record id for the stored image</returns>
        int SaveImageToDb(
            int imageDataId,
            DatabaseFileData fileData,
            string modifiedBy,int flareId, bool isRemoved);

        /// <summary>
        /// Deletes an image file from the database
        /// </summary>
        /// <param name="imageDataId">The record id for the stored image in the database.</param>
        void DeleteImageToDb(int imageDataId);
    }
}