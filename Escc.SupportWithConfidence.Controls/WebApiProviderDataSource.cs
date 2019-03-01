using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Threading.Tasks;
using Escc.Data.Ado;
using Escc.Net;
using Newtonsoft.Json;

namespace Escc.SupportWithConfidence.Controls
{
    /// <summary>
    /// Get data about Support with Confidence providers from a Web API
    /// </summary>
    public class WebApiProviderDataSource : IProviderDataSource
    {
        private static HttpClient _httpClient;
        private readonly Uri _apiBaseUrl;
        private IHttpClientProvider _httpClientProvider;

        /// <summary>
        /// Creates a new <see cref="WebApiProviderDataSource"/>
        /// </summary>
        /// <param name="apiBaseUrl">The base URL for the web API</param>
        /// <param name="httpClientProvider">A method of getting an <see cref="HttpClient"/> to use for making requests to the web API</param>
        public WebApiProviderDataSource(Uri apiBaseUrl, IHttpClientProvider httpClientProvider)
        {
            _apiBaseUrl = apiBaseUrl ?? throw new ArgumentNullException(nameof(apiBaseUrl));
            _httpClientProvider = httpClientProvider ?? throw new ArgumentNullException(nameof(httpClientProvider));
        }

        private void EnsureHttpClient()
        {
            if (_httpClient == null)
            {
                _httpClient = _httpClientProvider.GetHttpClient();
            }
        }

        /// <summary>
        /// Gets all categories, optionally limited to those with at least one approved provider.
        /// </summary>
        /// <param name="hasProvider">if set to <c>true</c> only select categories with at least one provider.</param>
        /// <returns></returns>
        public async Task<IEnumerable<Category>> GetAllCategoriesWithProvider(bool hasProvider)
        {
            EnsureHttpClient();

            var json = await _httpClient.GetStringAsync(new Uri(_apiBaseUrl.ToString().TrimEnd('/') + "/api/Categories?hasProvider=" + hasProvider));
            return JsonConvert.DeserializeObject<IEnumerable<Category>>(json);
        }


        /// <summary>
        /// Return provider using provider Id or only return the provider by provider Id if approved
        /// </summary>
        /// <param name="id"></param>
        /// <param name="thatIsApproved"></param>
        /// <returns></returns>
        public async Task<DataSet> GetProviderById(int id, bool thatIsApproved)
        {
            EnsureHttpClient();

            var json = await _httpClient.GetStringAsync(new Uri(_apiBaseUrl.ToString().TrimEnd('/') + "/api/Providers/" + id + "?approved=" + thatIsApproved));
            return JsonConvert.DeserializeObject<DataSet>(json);
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
        public async Task<DataSet> GetPagedResultsByCategoryId(int easting, int northing, int pageindex, int pagesize, int categoryId)
        {
            EnsureHttpClient();

            var json = await _httpClient.GetStringAsync(new Uri(_apiBaseUrl.ToString().TrimEnd('/') + "/api/Providers/?easting=" + easting + "&northing=" + northing + "&page=" + pageindex + "&pagesize=" + pagesize + "&category=" + categoryId));
            return JsonConvert.DeserializeObject<DataSet>(json);
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
        public async Task<DataSet> GetPagedResultsForSearchTerm(int pageindex, int pagesize, int easting, int northing, string searchTerm)
        {
            EnsureHttpClient();

            var json = await _httpClient.GetStringAsync(new Uri(_apiBaseUrl.ToString().TrimEnd('/') + "/api/Providers/?easting=" + easting + "&northing=" + northing + "&page=" + pageindex + "&pagesize=" + pagesize + "&search=" + searchTerm));
            return JsonConvert.DeserializeObject<DataSet>(json);
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
        public async Task<DatabaseFileData> GetImageFromDb(int imageDataId, bool includeBlobData)
        {
            EnsureHttpClient();

            var json = await _httpClient.GetStringAsync(new Uri(_apiBaseUrl.ToString().TrimEnd('/') + "/api/Images/" + imageDataId + "?includeBlobData=" + includeBlobData));
            return JsonConvert.DeserializeObject<DatabaseFileData>(json);
        }
    }
}