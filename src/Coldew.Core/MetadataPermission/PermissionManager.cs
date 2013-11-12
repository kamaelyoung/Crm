using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core.Organization;
using Coldew.Api;

namespace Coldew.Core.MetadataPermission
{
    public class PermissionManager
    {
        public PermissionManager(ColdewObject coldewObject)
        {
            this.ColdewObject = coldewObject;
            this.EntityPermissionManager = new MetadataEntityPermissionManager(coldewObject.ColdewManager.OrgManager);
            this.PermissionStrategyManager = new MetadataPermissionStrategyManager(coldewObject.ColdewManager);
        }

        public MetadataEntityPermissionManager EntityPermissionManager { private set; get; }

        public MetadataPermissionStrategyManager PermissionStrategyManager { private set; get; }
 
        public ColdewObject ColdewObject { private set; get; }

        public bool HasValue(User user, MetadataPermissionValue value, Metadata metadata)
        {
            if (this.EntityPermissionManager.HasValue(user, value, metadata))
            {
                return true;
            }
            if (this.PermissionStrategyManager.HasValue(this.ColdewObject.ID, user, value, metadata))
            {
                return true;
            }
            return false;
        }

        public MetadataPermissionValue GetValue(User user, Metadata metadata)
        {
            MetadataPermissionValue entityValue = this.EntityPermissionManager.GetPermission(user, metadata);

            MetadataPermissionValue strategyValue = this.PermissionStrategyManager.GetPermission(this.ColdewObject.ID, user, metadata);

            return (MetadataPermissionValue)((int)entityValue | (int)strategyValue);
        }
    }
}
