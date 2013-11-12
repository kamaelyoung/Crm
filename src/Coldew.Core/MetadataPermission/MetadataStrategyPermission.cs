using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core.Search;
using Coldew.Api;
using Coldew.Core.Organization;

namespace Coldew.Core.MetadataPermission
{
    public class MetadataStrategyPermission : MetadataPermission
    {
        public MetadataStrategyPermission(MetadataMember member, MetadataPermissionValue value, MetadataExpressionSearcher searcher)
            :base(member, value)
        {
            this.Searcher = searcher;
        }

        public MetadataExpressionSearcher Searcher { private set; get; }

        public override bool HasValue(Metadata metadata, User user, MetadataPermissionValue value)
        {
            return this.Value.HasFlag(value) && this.Member.Contains(metadata, user) && this.Searcher.Accord(user, metadata);
        }
    }
}
