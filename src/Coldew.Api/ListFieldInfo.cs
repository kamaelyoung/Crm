using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Api
{
    [Serializable]
    public class ListFieldInfo: FieldInfo
    {
        public string DefaultValue { set; get; }

        public List<string> SelectList { set; get; }
    }
}
