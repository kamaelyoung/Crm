using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Data
{
    public class MetadataPermissionStrategyModel
    {
        public virtual string ObjectId { set; get; }

        public virtual string PermissionJson { set; get; }
    }
}
