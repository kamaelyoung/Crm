using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm.Api
{
    [Serializable]
    public class MetadataInfo
    {
        public int ID { get; set; }

        public List<PropertyInfo> Propertys { set; get; }
    }
}
