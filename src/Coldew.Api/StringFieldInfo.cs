using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Api
{
    [Serializable]
    public class StringFieldInfo : FieldInfo
    {
        public string DefaultValue { set; get; }
    }
}
