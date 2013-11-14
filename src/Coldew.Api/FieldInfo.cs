using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Api
{
    [Serializable]
    public class FieldInfo
    {
        public int ID { set; get; }

        public string Code { set; get; }

        public string Name { set; get; }

        public string Tip { set; get; }

        public bool Required { set; get; }

        public string Type { set; get; }

        public string TypeName { set; get; }

        public bool IsSystem { set; get; }
    }
}
