using System.Collections.Generic;
using EsccWebTeam.SupportWithConfidence.Controls;

namespace Escc.SupportWithConfidence.Controls
{
    // Look at the querystring and decide how best to create the data for repeater to use
    public class SearchController
    {
        private readonly ResultMapper _mapper = new ResultMapper();


        public SearchController()
        {
            QueryStringParameters = new QueryParameter();
            CategoryHeading = string.Empty;
        }

        public string CategoryHeading { get; set; }

        public int TotalResults { get; private set; }


        public QueryParameter QueryStringParameters { get; set; }


        public IList<IResult> GetResults()
        {
            QueryStringParameters.Process();

            switch (QueryStringParameters.DataSearchCall)
            {
                case SearchCall.Category:

                    _mapper.Map(
                        DataAccess.GetPagedResultsByCategoryId(QueryStringParameters.Easting, QueryStringParameters.Northing,
                                                               QueryStringParameters.CurrentResultPage, QueryStringParameters.PageSize,
                                                               QueryStringParameters.CategoryId), QueryStringParameters);
                    TotalResults = _mapper.TotalResults;
                    CategoryHeading = _mapper.CategoryHeading;
                    break;
                case SearchCall.Provider:
                    _mapper.Map(
                        DataAccess.GetPagedResultsForSearchTerm(QueryStringParameters.CurrentResultPage, QueryStringParameters.PageSize,
                                                                QueryStringParameters.Easting, QueryStringParameters.Northing,
                                                                QueryStringParameters.ProviderSearchValue), QueryStringParameters);
                    TotalResults = _mapper.TotalResults;
                    break;
            }

            return _mapper.Collection;
        }
    }
}