using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Escc.SupportWithConfidence.Controls
{
    public class ProviderCategoryMapper
    {
        public ProviderCategoryMapper()
        {
            ProviderCategory = new Collection<ProviderCategory>();
            
        }

        
        public IList<ProviderCategory> ProviderCategory { get; set; }
    }
}