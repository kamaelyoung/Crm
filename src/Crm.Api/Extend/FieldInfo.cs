using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm.Api
{
    [Serializable]
    public class FieldInfo
    {
        public int ID { set; get; }

        public string Code { set; get; }

        public FormType FormType { set; get; }

        public string Name { set; get; }

        public bool Required { set; get; }

        public FieldType Type { set; get; }

        public bool CanModify { set; get; }

        public bool CanImport { set; get; }

        public int Index { set; get; }

        public PropertyValueType ValueType { set; get; }
    }
}
