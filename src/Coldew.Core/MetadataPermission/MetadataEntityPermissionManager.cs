using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Data;
using Newtonsoft.Json;
using Coldew.Core.Organization;
using Coldew.Api;
using System.Threading;

namespace Coldew.Core.MetadataPermission
{
    public class MetadataEntityPermissionManager
    {
        Dictionary<string, MetadataEntityPermission> _permissionDict;
        protected ReaderWriterLock _lock;
        OrganizationManagement _orgManager;

        public MetadataEntityPermissionManager(OrganizationManagement orgManager)
        {
            this._permissionDict = new Dictionary<string, MetadataEntityPermission>();
            this._orgManager = orgManager;
            this._lock = new ReaderWriterLock();
        }

        public MetadataEntityPermission GetPermission(string metadataId)
        {
            this._lock.AcquireReaderLock(0);
            try
            {
                if (this._permissionDict.ContainsKey(metadataId))
                {
                    return this._permissionDict[metadataId];
                }
                return null;
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public void SetPermission(string metadataId, List<MetadataMemberPermissionValue> values)
        {
            this._lock.AcquireWriterLock(0);
            try
            {
                if (this._permissionDict.ContainsKey(metadataId))
                {
                    MetadataEntityPermission permission = this._permissionDict[metadataId];
                    permission.SetValues(values);
                }
                else
                {
                    this.Create(metadataId, values);
                }
            }
            finally
            {
                this._lock.ReleaseWriterLock();
            }
        }

        private MetadataEntityPermission Create(string metadataId, List<MetadataMemberPermissionValue> values)
        {
            List<MemberPermissionValueJsonModel> valueModels = new List<MemberPermissionValueJsonModel>();
            foreach(MetadataMemberPermissionValue value in values)
            {
                valueModels.Add(new MemberPermissionValueJsonModel{ memberId = value.Member.ID, value = (int)value.Value });    
            }

            MetadataPermissionModel model = new MetadataPermissionModel();
            model.MetadataId = metadataId;
            model.PermissionJson = JsonConvert.SerializeObject(valueModels);

            NHibernateHelper.CurrentSession.Save(model);
            NHibernateHelper.CurrentSession.Flush();

            return this.Create(model);
        }

        private MetadataEntityPermission Create(MetadataPermissionModel model)
        {
            List<MemberPermissionValueJsonModel> valueModels = JsonConvert.DeserializeObject<List<MemberPermissionValueJsonModel>>(model.PermissionJson);
            List<MetadataMemberPermissionValue> values = new List<MetadataMemberPermissionValue>();
            foreach(MemberPermissionValueJsonModel valueModel in  valueModels )
            {
                Member member = this._orgManager.GetMember(valueModel.memberId);
                if(member != null)
                {
                    values.Add(new MetadataMemberPermissionValue(member, (MetadataPermissionValue)valueModel.value));
                }
            }
            MetadataEntityPermission permission = new MetadataEntityPermission(model.MetadataId, values);
            this._permissionDict.Add(permission.MetadataId, permission);
            return permission;
        }

        internal void Load()
        {
            this._lock.AcquireWriterLock(0);
            try
            {
                NHibernateHelper.CurrentSession.QueryOver<MetadataPermissionModel>().List();
            }
            finally
            {
                this._lock.ReleaseWriterLock();
            }
        }
    }
}
