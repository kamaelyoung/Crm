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
        Dictionary<string, List<MetadataEntityPermission>> _permissionDict;
        protected ReaderWriterLock _lock;
        OrganizationManagement _orgManager;

        public MetadataEntityPermissionManager(OrganizationManagement orgManager)
        {
            this._permissionDict = new Dictionary<string, List<MetadataEntityPermission>>();
            this._orgManager = orgManager;
            this._lock = new ReaderWriterLock();

            this.Load();
        }

        public MetadataPermissionValue GetPermission(User user, Metadata metadata)
        {
            this._lock.AcquireReaderLock(0);
            try
            {
                int value = 0;
                if (this._permissionDict.ContainsKey(metadata.ID))
                {
                    List<MetadataEntityPermission> permissions = this._permissionDict[metadata.ID];
                    foreach (MetadataEntityPermission permission in permissions)
                    {
                        if (permission.Member.Contains(metadata, user))
                        {
                            value = value | (int)permission.Value;
                        }
                    }
                }
                return (MetadataPermissionValue)value;
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public bool HasValue(User user, MetadataPermissionValue value, Metadata metadata)
        {
            this._lock.AcquireReaderLock(0);
            try
            {
                if (this._permissionDict.ContainsKey(metadata.ID))
                {
                    List<MetadataEntityPermission> permissions = this._permissionDict[metadata.ID];
                    foreach (MetadataEntityPermission permission in permissions)
                    {
                        if(permission.HasValue(metadata, user, value))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public MetadataEntityPermission Create(string metadataId, MetadataMember member, MetadataPermissionValue value)
        {
            this._lock.AcquireWriterLock(0);
            try
            {
                MetadataEntityPermissionModel model = new MetadataEntityPermissionModel();
                model.MetadataId = metadataId;
                model.Member = member.Serialize();
                model.Value = (int)value;

                model.ID = NHibernateHelper.CurrentSession.Save(model).ToString();
                NHibernateHelper.CurrentSession.Flush();

                return this.Create(model);
            }
            finally
            {
                this._lock.ReleaseWriterLock();
            }
        }

        private MetadataEntityPermission Create(MetadataEntityPermissionModel model)
        {
            MetadataMember metadataMember = MetadataMember.Create(model.Member, this._orgManager);
            if (metadataMember != null)
            {
                MetadataEntityPermission permission = new MetadataEntityPermission(model.ID, model.MetadataId, metadataMember, (MetadataPermissionValue)model.Value);
                if(!this._permissionDict.ContainsKey(model.MetadataId))
                {
                    this._permissionDict.Add(model.MetadataId, new List<MetadataEntityPermission>());
                }
                this._permissionDict[model.MetadataId].Add(permission);
                return permission;

            }
            return null;
        }

        internal void Load()
        {
            this._lock.AcquireWriterLock(0);
            try
            {
                IList<MetadataEntityPermissionModel> models = NHibernateHelper.CurrentSession.QueryOver<MetadataEntityPermissionModel>().List();
                foreach (MetadataEntityPermissionModel model in models)
                {
                    this.Create(model);
                }
            }
            finally
            {
                this._lock.ReleaseWriterLock();
            }
        }
    }
}
