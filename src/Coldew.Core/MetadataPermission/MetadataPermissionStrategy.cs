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
        public MetadataPermissionStrategy(string objectId, List<MetadataMemberPermissionStrategyValue> permissionValues)
        {
            this.ObjectId = objectId;
            this._values = permissionValues;
        }

        public string ObjectId { private set; get; }

        private List<MetadataMemberPermissionStrategyValue> _values;

        public void SetValues(List<MetadataMemberPermissionStrategyValue> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            List<MemberPermissionStrategyValueJsonModel> valueModels = new List<MemberPermissionStrategyValueJsonModel>();
            foreach (MetadataMemberPermissionStrategyValue permission in this._values)
            {
                valueModels.Add(new MemberPermissionStrategyValueJsonModel { memberId = permission.Member.ID, value = (int)permission.Value, searchExpression = permission.Searcher.ToString() });
            }

            MetadataPermissionStrategyModel model = NHibernateHelper.CurrentSession.Get<MetadataPermissionStrategyModel>(this.ObjectId);
            model.PermissionJson = JsonConvert.SerializeObject(valueModels);

            NHibernateHelper.CurrentSession.Update(model);
            NHibernateHelper.CurrentSession.Flush();

            this._values = values;
        }

        public bool HasValue(Metadata metadata, User user, MetadataPermissionValue value)
        {
            foreach (MetadataMemberPermissionStrategyValue permission in this._values)
            {
                if (permission.Member.Contains(user) && permission.Value.HasFlag(value) && permission.Searcher.Accord(user, metadata))
                {
                    return true;
                }
            }
            return false;
        }

        public MetadataPermissionValue GetValue(Metadata metadata, User user)
        {
            int value = 0;

            foreach (MetadataMemberPermissionStrategyValue permission in this._values)
            {
                if (permission.Member.Contains(user) && permission.Searcher.Accord(user, metadata))
                {
                    value = value | (int)permission.Value;
                }
            }

            return (MetadataPermissionValue)value;
        }
    }
}
