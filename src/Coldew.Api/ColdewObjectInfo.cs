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

        public string Code { set; get; }

        public string Name { set; get; }

        public ColdewObjectType Type { set; get; }

        public List<FieldInfo> Fields { set; get; }

        public ObjectPermissionValue PermissionValue { set; get; }

        public FieldInfo GetField(string code)
        {
            return this.Fields.Find(x => x.Code == code);
        }
    }
}
