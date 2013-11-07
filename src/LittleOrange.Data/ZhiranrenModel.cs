using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Data;

namespace LittleOrange.Data
{
    public class ZhiranrenModel : MetadataModel
    {
        public virtual string ID { set; get; }

        public virtual string PropertysJson { get; set; }
    }
}
