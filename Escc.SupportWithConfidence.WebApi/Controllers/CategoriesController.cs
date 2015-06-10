using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Escc.SupportWithConfidence.Controls;
using Exceptionless;

namespace Escc.SupportWithConfidence.WebApi.Controllers
{
    public class CategoriesController : ApiController
    {
        /// <summary>
        /// Gets all categories, optionally limited to those with at least one approved provider.
        /// </summary>
        /// <param name="hasProvider">if set to <c>true</c> only select categories with at least one provider.</param>
        /// <returns></returns>
        [HttpGet]
        public DataSet GetAll(bool hasProvider = true)
        {
            try
            {
                var dataSource = new SqlServerProviderDataSource();
                return dataSource.GetAllCategoriesWithProvider(hasProvider);
            }
            catch (Exception e)
            {
                e.ToExceptionless().Submit();
                throw;
            }
        }
    }
}
