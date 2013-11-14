using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core.Organization;

namespace Coldew.Core
{
    public class CreatedUserField : UserField
    {
        public CreatedUserField(FieldNewInfo info, UserManagement userManager)
            : base(info, false, userManager)
        {
            
        }
    }
}
