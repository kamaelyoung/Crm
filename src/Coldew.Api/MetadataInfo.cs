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
            get
            {
                PropertyInfo info = this.Propertys.Find(x => x.Code == FormConstCode.FIELD_NAME_NAME);
                return info.ShowValue;
            }
        }

        public List<PropertyInfo> Propertys { set; get; }
    }
}
