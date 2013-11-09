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
            MetadataEntityPermission entityPermission = this.EntityPermissionManager.GetPermission(metadata.ID);
            if (entityPermission != null && entityPermission.HasValue(user, value))
            {
                return true;
            }

            MetadataPermissionStrategy strategy = this.PermissionStrategyManager.GetPermission(this.ColdewObject.ID);
            if (strategy != null && strategy.HasValue(metadata, user, value))
            {
                return true;
            }
            return false;
        }

        public MetadataPermissionValue GetValue(User user, Metadata metadata)
        {
            MetadataEntityPermission entityPermission = this.EntityPermissionManager.GetPermission(metadata.ID);
            MetadataPermissionValue entityValue = entityPermission.GetValue(user);

            MetadataPermissionStrategy strategy = this.PermissionStrategyManager.GetPermission(this.ColdewObject.ID);
            MetadataPermissionValue strategyValue = strategy.GetValue(metadata, user);

            return (MetadataPermissionValue)((int)entityValue | (int)strategyValue);
        }
    }
}
