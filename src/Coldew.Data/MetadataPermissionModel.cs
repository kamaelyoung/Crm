using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Data
{
    public class MetadataPermissionModel
    {
        public virtual string MetadataId { set; get; }

        public virtual string PermissionJson { set; get; }
    }
}
