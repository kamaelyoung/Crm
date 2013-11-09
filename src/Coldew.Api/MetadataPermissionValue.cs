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
        Create = 1,
        Modify = 2,
        View = 4,
        Delete = 8,
        Export = 16,
        Manage = 32
    }
}
