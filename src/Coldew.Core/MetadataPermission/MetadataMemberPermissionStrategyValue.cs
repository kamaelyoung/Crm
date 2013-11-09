using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core.Search;
using Coldew.Api;
using Coldew.Core.Organization;

namespace Coldew.Core.MetadataPermission
{
    public class MetadataMemberPermissionStrategyValue : MetadataMemberPermissionValue
    {
        public MetadataMemberPermissionStrategyValue(Member member, MetadataPermissionValue value, MetadataExpressionSearcher searcher)
            :base(member, value)
        {
            this.Searcher = searcher;
        }

        public MetadataExpressionSearcher Searcher { private set; get; }
    }
}
