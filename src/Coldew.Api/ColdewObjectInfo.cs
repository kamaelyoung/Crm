using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Api
{
    [Serializable]
    public class ColdewObjectInfo
    {
        public string ID { set; get; }

        public string Name { set; get; }

        public List<FieldInfo> Fields { set; get; }
    }
}
