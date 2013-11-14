using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Api
{
    public enum ObjectPermissionValue
    {

        None = 0,
        View = 1,
        Create = 1 << 1,
        Export = 1 << 2,
        Import = 1 << 3,
        PermissionSetting = 1 << 4,
        All = ObjectPermissionValue.View | ObjectPermissionValue.Create | ObjectPermissionValue.Export 
            | ObjectPermissionValue.Import | ObjectPermissionValue.PermissionSetting
    }
}
