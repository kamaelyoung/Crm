using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm.Core.Extend
{
    public class ListFieldConfigJsonModel
    {
        public string DefaultValue { set; get; }

        public List<string> SelectList { set; get; }
    }
}
