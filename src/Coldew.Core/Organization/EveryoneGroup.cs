using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Coldew.Data.Organization;

namespace Coldew.Core.Organization
{
    public class EveryoneGroup : VirtualGroup
    {
        OrganizationManagement _organizationManager;
        public EveryoneGroup(GroupModel groupModel, OrganizationManagement organizationManager)
            : base(groupModel, organizationManager)
        {
            this._organizationManager = organizationManager;
        }

        public override ReadOnlyCollection<User> Users
        {
            get
            {
                return this._organizationManager
                    .UserManager
                    .Users
                    .Where(x => x != this._organizationManager.System)
                    .ToList()
                    .AsReadOnly();
            }
        }
    }
}
