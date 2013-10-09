using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Api
{
    [Serializable]
    public class PropertyInfo
    {
        public string Code { set; get; }

        public string ShowValue { set; get; }

        public dynamic EditValue { set; get; }
    }
}
