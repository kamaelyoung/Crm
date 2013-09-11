using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Api;

namespace Crm.Core.Extend
{
    public class MetadataSearcher
    {
        List<PropertySearchInfo> _searchInfos;
        public MetadataSearcher(List<PropertySearchInfo> searchInfos)
        {
            this._searchInfos = searchInfos;
        }

        public bool Accord(Metadata metadata)
        {
            foreach (PropertySearchInfo serachInfo in this._searchInfos)
            {
                MetadataProperty property = metadata.GetProperty(serachInfo.FieldCode);
                if (property == null || !property.Value.InCondition(serachInfo.Condition))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
