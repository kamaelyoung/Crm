using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api;
using Coldew.Core.Organization;

namespace Coldew.Core.MetadataPermission
{
    public class MetadataPermission
    {
        public MetadataPermission(MetadataMember member, MetadataPermissionValue value)
        {
            this.Member = member;
            this.Value = value;
        }

        public MetadataPermissionValue Value { private set; get; }

        public MetadataMember Member { private set; get; }

        public virtual bool HasValue(Metadata metadata, User user, MetadataPermissionValue value)
        {
            return this.Member.Contains(metadata, user) && this.Value.HasFlag(value);
        }
    }
}
