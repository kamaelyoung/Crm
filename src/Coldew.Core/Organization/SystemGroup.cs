using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api.Organization.Exceptions;
using Coldew.Data.Organization;
using Coldew.Api.Organization;

namespace Coldew.Core.Organization
{
    public class SystemGroup : Group
    {
        public SystemGroup(GroupModel groupModel, OrganizationManagement organizationManager)
            : base(groupModel, organizationManager)
        {
            
        }
    }
}
