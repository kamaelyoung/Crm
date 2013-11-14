using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core.Organization;
using Coldew.Api;

namespace Coldew.Core.Permission
{
    public class MetadataPermissionManager
    {
        public MetadataPermissionManager(ColdewObject cobject)
        {
            this.EntityManager = new MetadataEntityPermissionManager(cobject);
            this.StrategyManager = new MetadataPermissionStrategyManager(cobject);
        }

        public MetadataEntityPermissionManager EntityManager { private set; get; }

        public MetadataPermissionStrategyManager StrategyManager { private set; get; }

        public bool HasValue(User user, MetadataPermissionValue value, Metadata metadata)
        {
            if (this.EntityManager.HasValue(user, value, metadata))
            {
                return true;
            }
            if (this.StrategyManager.HasValue(user, value, metadata))
            {
                return true;
            }
            return false;
        }

        public MetadataPermissionValue GetValue(User user, Metadata metadata)
        {
            MetadataPermissionValue entityValue = this.EntityManager.GetPermission(user, metadata);

            MetadataPermissionValue strategyValue = this.StrategyManager.GetPermission(user, metadata);

            return (MetadataPermissionValue)((int)entityValue | (int)strategyValue);
        }
    }
}
