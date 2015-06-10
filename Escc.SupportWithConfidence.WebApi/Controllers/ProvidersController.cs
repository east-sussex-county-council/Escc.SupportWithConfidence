using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Escc.Data.Ado;
using Escc.SupportWithConfidence.Controls;
using Exceptionless;

namespace Escc.SupportWithConfidence.WebApi.Controllers
{
    public class ProvidersController : ApiController
    {
        /// <summary>
        /// Return provider using provider Id or only return the provider by provider Id if approved
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="approved">if set to <c>true</c> only get an approved provider.</param>
        /// <returns></returns>
        [HttpGet]
        public DataSet ByProviderId(int id, bool approved=true)
        {
            try
            {
                var dataSource = new SqlServerProviderDataSource();
                return dataSource.GetProviderById(id, approved);
            }
            catch (Exception e)
            {
                e.ToExceptionless().Submit();
                throw;
            }
        }

        /// <summary>
        /// Gets a page of approved providers matching a category identifier.
        /// </summary>
        /// <param name="easting">The easting.</param>
        /// <param name="northing">The northing.</param>
        /// <param name="page">The page number.</param>
        /// <param name="pagesize">The pagesize.</param>
        /// <param name="category">The category identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public DataSet GetAll(int easting, int northing, int page, int pagesize, int category)
        {
            try
            {
                var dataSource = new SqlServerProviderDataSource();
                return dataSource.GetPagedResultsByCategoryId(easting, northing, page, pagesize, category);
            }
            catch (Exception e)
            {
                e.ToExceptionless().Submit();
                throw;
            }
        }

        /// <summary>
        /// Gets a page of approved providers matching a search term.
        /// </summary>
        /// <param name="easting">The easting.</param>
        /// <param name="northing">The northing.</param>
        /// <param name="page">The page number.</param>
        /// <param name="pagesize">The pagesize.</param>
        /// <param name="search">The search term.</param>
        /// <returns></returns>
        public DataSet GetAll(int easting, int northing, int page, int pagesize, string search)
        {
            try
            {
                var dataSource = new SqlServerProviderDataSource();
                return dataSource.GetPagedResultsForSearchTerm(page, pagesize, easting, northing, search);
            }
            catch (Exception e)
            {
                e.ToExceptionless().Submit();
                throw;
            }
        }
    }
}
