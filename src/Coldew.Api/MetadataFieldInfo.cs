using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Api
{
    [Serializable]
    public class MetadataFieldInfo : FieldInfo
    {
        public string ValueFormName { set; get; }

        public string ValueFormId { set; get; }
    }
}
