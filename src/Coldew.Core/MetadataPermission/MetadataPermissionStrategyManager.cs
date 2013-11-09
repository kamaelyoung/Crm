using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core.Organization;
using System.Threading;
using Coldew.Data;
using Newtonsoft.Json;
using Coldew.Api;
using Coldew.Core.Search;

namespace Coldew.Core.MetadataPermission
{
    public class MetadataPermissionStrategyManager
    {
        Dictionary<string, MetadataPermissionStrategy> _permissionDict;
        protected ReaderWriterLock _lock;
        OrganizationManagement _orgManager;
        ColdewManager _coldewManager;

        public MetadataPermissionStrategyManager(ColdewManager coldewManager)
        {
            this._permissionDict = new Dictionary<string, MetadataPermissionStrategy>();
            this._orgManager = coldewManager.OrgManager;
            this._coldewManager = coldewManager;
            this._lock = new ReaderWriterLock();
        }

        public MetadataPermissionStrategy GetPermission(string objectId)
        {
            this._lock.AcquireReaderLock(0);
            try
            {
                if (this._permissionDict.ContainsKey(objectId))
                {
                    return this._permissionDict[objectId];
                }
                return null;
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public void SetPermission(string objectId, List<MetadataMemberPermissionStrategyValue> values)
        {
            this._lock.AcquireWriterLock(0);
            try
            {
                if (this._permissionDict.ContainsKey(objectId))
                {
                    MetadataPermissionStrategy permission = this._permissionDict[objectId];
                    permission.SetValues(values);
                }
                else
                {
                    this.Create(objectId, values);
                }
            }
            finally
            {
                this._lock.ReleaseWriterLock();
            }
        }

        private MetadataPermissionStrategy Create(string objectId, List<MetadataMemberPermissionStrategyValue> values)
        {
            List<MemberPermissionStrategyValueJsonModel> valueModels = new List<MemberPermissionStrategyValueJsonModel>();
            foreach (MetadataMemberPermissionStrategyValue value in values)
            {
                valueModels.Add(new MemberPermissionStrategyValueJsonModel { memberId = value.Member.ID, value = (int)value.Value, searchExpression = value.Searcher.ToString() });
            }

            MetadataPermissionStrategyModel model = new MetadataPermissionStrategyModel();
            model.ObjectId = objectId;
            model.PermissionJson = JsonConvert.SerializeObject(valueModels);

            NHibernateHelper.CurrentSession.Save(model);
            NHibernateHelper.CurrentSession.Flush();

            return this.Create(model);
        }

        private MetadataPermissionStrategy Create(MetadataPermissionStrategyModel model)
        {
            ColdewObject cobject = this._coldewManager.ObjectManager.GetFormById(model.ObjectId);
            if (cobject != null)
            {
                List<MemberPermissionStrategyValueJsonModel> valueModels = JsonConvert.DeserializeObject<List<MemberPermissionStrategyValueJsonModel>>(model.PermissionJson);
                List<MetadataMemberPermissionStrategyValue> values = new List<MetadataMemberPermissionStrategyValue>();
                foreach (MemberPermissionStrategyValueJsonModel valueModel in valueModels)
                {
                    Member member = this._orgManager.GetMember(valueModel.memberId);
                    if (member != null)
                    {
                        values.Add(new MetadataMemberPermissionStrategyValue(member, (MetadataPermissionValue)valueModel.value, MetadataExpressionSearcher.Parse(valueModel.searchExpression, cobject)));
                    }
                }
                MetadataPermissionStrategy permission = new MetadataPermissionStrategy(model.ObjectId, values);
                this._permissionDict.Add(permission.ObjectId, permission);
                return permission;
            } 
            return null;
        }

        internal void Load()
        {
            this._lock.AcquireWriterLock(0);
            try
            {
                NHibernateHelper.CurrentSession.QueryOver<MetadataPermissionStrategyModel>().List();
            }
            finally
            {
                this._lock.ReleaseWriterLock();
            }
        }
    }
}
