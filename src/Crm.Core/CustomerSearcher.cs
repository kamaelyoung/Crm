using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Api;
using Crm.Core.Extend;
using System.Text.RegularExpressions;

namespace Crm.Core
{
    public class CustomerSearcher
    {
        CustomerSearchInfo _searchInfo;
        MetadataSearcher _metadataSearcher;
        List<Regex> _keywordRegexs;
        public CustomerSearcher(CustomerSearchInfo searchInfo)
        {
            this._searchInfo = searchInfo;
            this._keywordRegexs  = RegexHelper.GetRegexes(searchInfo.Keywords);
            if (searchInfo.PropertySearchInfos != null && searchInfo.PropertySearchInfos.Count > 0)
            {
                this._metadataSearcher = new MetadataSearcher(searchInfo.PropertySearchInfos);
            }
        }

        public bool Accord(Customer customer)
        {

            if (this._keywordRegexs.Any(regex => !regex.IsMatch(customer.Content)))
            {
                return false;
            }

            if (this._metadataSearcher != null && !this._metadataSearcher.Accord(customer.Metadata))
            {
                return false;
            }

            return true;
        }
    }
}
