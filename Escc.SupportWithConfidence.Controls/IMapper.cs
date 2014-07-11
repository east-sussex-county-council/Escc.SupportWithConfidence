using System.Collections.Generic;
using System.Data;
using EsccWebTeam.SupportWithConfidence.Controls;

namespace Escc.SupportWithConfidence.Controls
{
    interface IMapper
    {
         IList<IResult> Collection { get; set; }
                
         void Map(DataSet dataSet, QueryParameter queryParameters);
    }


}
