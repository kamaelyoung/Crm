using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api;

namespace Crm.Api
{
    [Serializable]
    public class ContractSearchInfo
    {
        public List<string> Keywords { set; get; }
        public DateRange StartDateRange { set; get; }
        public DateRange EndDateRange { set; get; }
        public FloagRange ValueRange { set; get; }
    }
}
