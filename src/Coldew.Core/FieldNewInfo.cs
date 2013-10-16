using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api;

namespace Coldew.Core
{
    public class FieldNewInfo
    {
        public FieldNewInfo(int id, string code, string name, bool required, bool canModify,
            string type, bool canInput, int index, ColdewObject form)
        {
            this.ID = id;
            this.Name = name;
            this.Required = required;
            this.CanModify = canModify;
            this.Code = code;
            this.CanInput = canInput;
            this.Index = index;
            this.Type = type;
            this.ColdewObject = form;
        }

        public int ID { set; get; }

        public string Code { set; get; }

        public string Name { set; get; }

        public bool Required { set; get; }

        public bool CanModify { set; get; }

        public bool CanInput { set; get; }

        public int Index { set; get; }

        public string Type { set; get; }

        public ColdewObject ColdewObject { private set; get; }
    }
}
