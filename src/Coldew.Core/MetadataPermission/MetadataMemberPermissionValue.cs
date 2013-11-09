using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api;
using Coldew.Core.Organization;

namespace Coldew.Core.MetadataPermission
{
    public class MetadataMemberPermissionValue
    {
        public MetadataMemberPermissionValue(Member member, MetadataPermissionValue value)
        {
            this.Member = member;
            this.Value = value;
        }

        public MetadataPermissionValue Value { private set; get; }

        public Member Member { private set; get; }
    }
}
