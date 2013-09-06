using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm.Api
{
    [Serializable]
    public class CustomerSearchInfo
    {
        public List<string> Keywords { set; get; }
        public List<PropertySearchInfo> PropertySearchInfos {set;get;}
    }
}
