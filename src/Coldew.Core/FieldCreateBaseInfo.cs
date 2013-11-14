using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Core
{
    public class FieldCreateBaseInfo
    {
        public FieldCreateBaseInfo(string code, string name, string tip, bool required, bool isSystem)
        {
            this.Code = code;
            this.Name = name;
            this.Tip = tip;
            this.Required = required;
            this.IsSystem = isSystem;
        }

        public string Code { set; get; }

        public string Name { set; get; }

        public string Tip { set; get; }

        public bool Required { set; get; }

        public bool IsSystem { set; get; }
    }
}
