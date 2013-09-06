using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Api;
using System.Text.RegularExpressions;

namespace Crm.Core
{
    public class ContractSearcher
    {
        ContractSearchInfo _searchInfo;
        List<Regex> _keywordRegexs;
        public ContractSearcher(ContractSearchInfo searchInfo)
        {
            this._searchInfo = searchInfo;
            this._keywordRegexs  = RegexHelper.GetRegexes(searchInfo.Keywords);
        }

        public bool Accord(Contract contract)
        {
            if (this._searchInfo.StartDateRange != null && !this._searchInfo.StartDateRange.InRange(contract.StartDate))
            {
                return false;
            }

            if (this._searchInfo.EndDateRange != null && !this._searchInfo.EndDateRange.InRange(contract.EndDate))
            {
                return false;
            }

            if (this._searchInfo.ValueRange != null && !this._searchInfo.ValueRange.InRange(contract.Value))
            {
                return false;
            }

            if (this._keywordRegexs.Any(regex => !regex.IsMatch(contract.Content)))
            {
                return false;
            }

            return true;
        }
    }
}
