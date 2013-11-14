using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api;

namespace Coldew.Core
{
    public class FieldNewInfo
    {
        public FieldNewInfo(int id, string code, string name, string tip, bool required,
            string type, bool isSystem, ColdewObject form)
        {
            this.ID = id;
            this.Name = name;
            this.Tip = tip;
            this.Required = required;
            this.IsSystem = isSystem;
            this.Code = code;
            this.Type = type;
            this.ColdewObject = form;
        }

        public int ID { set; get; }

        public string Code { set; get; }

        public string Name { set; get; }

        public string Tip { set; get; }

        public bool Required { set; get; }

        public bool IsSystem { set; get; }

        public string Type { set; get; }

        public ColdewObject ColdewObject { private set; get; }
    }
}
