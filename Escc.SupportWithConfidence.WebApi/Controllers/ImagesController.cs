using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Escc.Data.Ado;
using Escc.SupportWithConfidence.Controls;
using Exceptionless;

namespace Escc.SupportWithConfidence.WebApi.Controllers
{
    /// <summary>
    /// API for retrieving images associated with provider records
    /// </summary>
    public class ImagesController : ApiController
    {
        /// <summary>
        /// Gets an image file from the database
        /// </summary>
        /// <param name="id">The record id for the stored image</param>
        /// <param name="includeBlobData">If false, a special stored procedure is called that gets just the file details without the actual BLOB data.
        /// If true, the normal file retrieval stored procedure is called.</param>
        /// <returns>
        /// The file data for the stored image
        /// </returns>
        [HttpGet]
        public DatabaseFileData ImageById(int id, bool includeBlobData = false)
        {
            try
            {
                var dataSource = new SqlServerProviderDataSource();
                return dataSource.GetImageFromDb(id, includeBlobData);
            }
            catch (Exception e)
            {
                e.ToExceptionless().Submit();
                throw;
            }
        }
    }
}
