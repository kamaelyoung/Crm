using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm.Api
{
    [Serializable]
    public class ListFieldConfigInfo: FieldConfigInfo
    {
        public string DefaultValue { set; get; }

        public List<string> SelectList { set; get; }
    }
}
