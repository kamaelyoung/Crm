using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Api
{
    [Serializable]
    public class MetadataInfo
    {
        public string ID { get; set; }

        public string Name
        {
            set;
            get;
        }

        public bool Favorited { set; get; }

        public MetadataPermissionValue PermissionValue { set; get; }

        public List<PropertyInfo> Propertys { set; get; }

        public PropertyInfo GetProperty(string code)
        {
            return this.Propertys.Find(x => x.Code == code);
        }
    }
}
