using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Api
{
    [Flags]
    public enum MetadataPermissionValue
    {
        None = 0,
        View = 1,
        Modify = 1 << 1,
        Delete = 1 << 2,
        All = MetadataPermissionValue.Modify | MetadataPermissionValue.View | MetadataPermissionValue.Delete
    }
}
