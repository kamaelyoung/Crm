using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Api
{
    public enum FieldPermissionValue
    {

        None = 0,
        View = 1,
        Edit = 1 << 1,
        All = FieldPermissionValue.View | FieldPermissionValue.Edit 
    }
}
