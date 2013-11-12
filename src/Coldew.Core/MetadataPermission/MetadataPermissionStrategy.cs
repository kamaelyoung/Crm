using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core.Search;
using Coldew.Api;
using Coldew.Core.Organization;
using Coldew.Data;
using Newtonsoft.Json;

namespace Coldew.Core.MetadataPermission
{
    public class MetadataPermissionStrategy
    {
        public MetadataPermissionStrategy(string id, string objectId, MetadataMember member, MetadataPermissionValue value, MetadataExpressionSearcher searcher)
        {
            this.ID = id;
            this.ObjectId = objectId; 
            this.Member = member;
            this.Value = value;
            this.Searcher = searcher;
        }

        public string ID { private set; get; }

        public string ObjectId { private set; get; }

        public MetadataPermissionValue Value { private set; get; }

        public MetadataMember Member { private set; get; }

        public MetadataExpressionSearcher Searcher { private set; get; }

        public virtual bool HasValue(Metadata metadata, User user, MetadataPermissionValue value)
        {
            if (this.Searcher == null)
            {
                return this.Value.HasFlag(value) && this.Member.Contains(metadata, user);
            }
            else
            {
                return this.Value.HasFlag(value) && this.Member.Contains(metadata, user) && this.Searcher.Accord(user, metadata);
            }
        }
    }
}
