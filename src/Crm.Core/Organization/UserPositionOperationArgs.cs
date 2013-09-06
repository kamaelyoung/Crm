using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Api.Organization;

namespace Crm.Core.Organization
{
    public class UserPositionOperationArgs
    {
        public UserPositionOperationArgs(User operatioinUser)
        {
            this.OperationUser = operatioinUser;
        }

        public UserPositionOperationArgs(User operationalUser, UserPositionInfo userPositionInfo)
            : this(operationalUser)
        {
            this.UserPositionInfo = userPositionInfo;
        }

        public User OperationUser { set; get; }

        public UserPositionInfo UserPositionInfo { set; get; }
    }
}
