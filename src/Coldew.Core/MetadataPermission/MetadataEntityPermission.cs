using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core.Organization;
using Coldew.Api;
using Coldew.Data;
using Newtonsoft.Json;

namespace Coldew.Core.MetadataPermission
{
    public class MetadataEntityPermission
    {

        List<MetadataMemberPermissionValue> _values;

        public MetadataEntityPermission(string metadataId, List<MetadataMemberPermissionValue> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }
            this.MetadataId = metadataId;
            this._values = values;
        }

        public string MetadataId { private set; get; }

        public void SetValues(List<MetadataMemberPermissionValue> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            List<MemberPermissionValueJsonModel> valueModels = new List<MemberPermissionValueJsonModel>();
            foreach(MetadataMemberPermissionValue permission in this._values)
            {
                valueModels.Add(new MemberPermissionValueJsonModel{ memberId = permission.Member.ID, value = (int)permission.Value });    
            }

            MetadataPermissionModel model = NHibernateHelper.CurrentSession.Get<MetadataPermissionModel>(this.MetadataId);
            model.PermissionJson = JsonConvert.SerializeObject(valueModels);

            NHibernateHelper.CurrentSession.Update(model);
            NHibernateHelper.CurrentSession.Flush();

            this._values = values;
        }

        public bool HasValue(User user, MetadataPermissionValue value)
        {
            foreach (MetadataMemberPermissionValue permission in this._values)
            {
                if (permission.Member.Contains(user) && permission.Value.HasFlag(value))
                {
                    return true;
                }
            }
            return false;
        }

        public MetadataPermissionValue GetValue(User user)
        {
            int value = 0;

            foreach (MetadataMemberPermissionValue permission in this._values)
            {
                if (permission.Member.Contains(user))
                {
                    value = value | (int)permission.Value;
                }
            }

            return (MetadataPermissionValue)value;
        }
    }
}
