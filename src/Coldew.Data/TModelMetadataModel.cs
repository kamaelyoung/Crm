using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Data
{
    public abstract class TModelMetadataModel
    {
        public virtual string ID { get; set; }

        public virtual string PropertysJson { get; set; }
    }
}
