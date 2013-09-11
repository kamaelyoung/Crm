using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Api;

namespace Crm.Core.Extend
{
    public class FieldNewInfo
    {
        public FieldNewInfo(int id, string code, string name, bool required, bool canModify,
            FieldType type, bool canImport, int index, Form form)
        {
            this.ID = id;
            this.Name = name;
            this.Required = required;
            this.CanModify = canModify;
            this.Code = code;
            this.CanImport = canImport;
            this.Index = index;
            this.Type = type;
            this.Form = form;
        }

        public int ID { set; get; }

        public string Code { set; get; }

        public string Name { set; get; }

        public bool Required { set; get; }

        public bool CanModify { set; get; }

        public bool CanImport { set; get; }

        public int Index { set; get; }

        public FieldType Type { set; get; }

        public Form Form { private set; get; }
    }
}
