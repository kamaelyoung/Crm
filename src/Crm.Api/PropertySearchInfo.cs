using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm.Api
{
    [Serializable]
    public class PropertySearchInfo
    {
        public string FieldCode { set; get; }

        public PropetySearchCondition Condition { set; get; }
    }
}
