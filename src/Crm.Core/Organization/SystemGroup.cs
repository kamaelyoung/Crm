using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Api.Organization.Exceptions;
using Crm.Data.Organization;
using Crm.Api.Organization;

namespace Crm.Core.Organization
{
    public class SystemGroup : Group
    {
        public SystemGroup(GroupModel groupModel, OrganizationManagement organizationManager)
            : base(groupModel, organizationManager)
        {
            
        }
    }
}
