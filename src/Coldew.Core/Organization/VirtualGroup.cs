﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Data.Organization;
using Coldew.Api.Organization.Exceptions;

namespace Coldew.Core.Organization
{
    public class VirtualGroup : Group
    {
        public VirtualGroup(GroupModel groupModel, OrganizationManagement organizationManager)
            : base(groupModel, organizationManager)
        {
            
        }

        public override void AddDepartment(User operationUser, Department department)
        {
            throw new CannotAddMemberToVirtualGroupException(this.Name);
        }

        public override void AddGroup(User operationUser, Group group)
        {
            throw new CannotAddMemberToVirtualGroupException(this.Name);
        }

        public override void AddPosition(User operationUser, Position position)
        {
            throw new CannotAddMemberToVirtualGroupException(this.Name);
        }

        public override void AddUser(User operationUser, User user)
        {
            throw new CannotAddMemberToVirtualGroupException(this.Name);
        }
    }
}
