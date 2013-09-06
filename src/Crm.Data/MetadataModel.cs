using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm.Data
{
    public class MetadataModel
    {
        public virtual int ID { get; set; }

        public virtual string PropertysJson { get; set; }
    }
}
