using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm.Data
{
    public class ListFieldConfigModel
    {
        public string DefaultValue { set; get; }

        public List<string> SelectList { set; get; }
    }
}
