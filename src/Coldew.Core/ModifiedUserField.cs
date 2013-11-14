using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core.Organization;

namespace Coldew.Core
{
    public class ModifiedUserField : UserField
    {
        public ModifiedUserField(FieldNewInfo info, UserManagement userManager)
            : base(info, false, userManager)
        {
            
        }
    }
}
