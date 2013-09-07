using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm.Api
{
    [Serializable]
    public class PropertyInfo
    {
        public string Code { set; get; }

        public string Value { set; get; }

        public dynamic EditValue { set; get; }
    }
}
