using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Data;
using Newtonsoft.Json;
using Coldew.Core.Organization;
using Coldew.Api;
using System.Threading;

namespace Coldew.Core.Permission
{
    public class MetadataEntityPermissionManager
    {
        List<MetadataEntityPermission> _permissions;
        protected ReaderWriterLock _lock;
        OrganizationManagement _orgManager;
        ColdewObject _cobject;

        public MetadataEntityPermissionManager(ColdewObject cobject)
        {
            this._cobject = cobject;
            this._permissions = new List<MetadataEntityPermission>();
            this._orgManager = cobject.ColdewManager.OrgManager;
            this._lock = new ReaderWriterLock();
        }

        public MetadataPermissionValue GetPermission(User user, Metadata metadata)
        {
            this._lock.AcquireReaderLock(0);
            try
            {
                int value = 0;
                foreach (MetadataEntityPermission permission in this._permissions)
                {
                    if (permission.Member.Contains(metadata, user))
                    {
                        value = value | (int)permission.Value;
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
                foreach (MetadataEntityPermission permission in this._permissions)
                {
                    if (permission.HasValue(metadata, user, value))
                    {
                        return true;
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
                model.ObjectId = this._cobject.ID;
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
            MetadataMember metadataMember = MetadataMember.Create(model.Member, this._cobject);
            if (metadataMember != null)
            {
                MetadataEntityPermission permission = new MetadataEntityPermission(model.ID, model.MetadataId, metadataMember, (MetadataPermissionValue)model.Value);
                this._permissions.Add(permission);
                return permission;

            }
            return null;
        }

        internal void Load()
        {
            this._lock.AcquireWriterLock(0);
            try
            {
                IList<MetadataEntityPermissionModel> models = NHibernateHelper.CurrentSession.QueryOver<MetadataEntityPermissionModel>().Where(x => x.ObjectId == this._cobject.ID).List();
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
