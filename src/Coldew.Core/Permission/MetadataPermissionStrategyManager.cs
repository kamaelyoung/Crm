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

namespace Coldew.Core.Permission
{
    public class MetadataPermissionStrategyManager
    {
        List<MetadataPermissionStrategy> _permissions;
        protected ReaderWriterLock _lock;
        OrganizationManagement _orgManager;
        ColdewManager _coldewManager;
        ColdewObject _cobject;

        public MetadataPermissionStrategyManager(ColdewObject cobject)
        {
            this._cobject = cobject;
            this._permissions = new List<MetadataPermissionStrategy>();
            this._orgManager = cobject.ColdewManager.OrgManager;
            this._coldewManager = cobject.ColdewManager;
            this._lock = new ReaderWriterLock();
        }

        public MetadataPermissionValue GetPermission(User user, Metadata metadata)
        {
            this._lock.AcquireReaderLock(0);
            try
            {
                int value = 0;
                foreach (MetadataPermissionStrategy permission in this._permissions)
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
                foreach (MetadataPermissionStrategy permission in this._permissions)
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

        public MetadataPermissionStrategy Create(MetadataMember member, MetadataPermissionValue value, string searchExpressions)
        {
            this._lock.AcquireWriterLock(0);
            try
            {
                MetadataPermissionStrategyModel model = new MetadataPermissionStrategyModel();
                model.ObjectId = this._cobject.ID;
                model.Member = member.Serialize();
                model.Value = (int)value;
                model.SearchExpressions = searchExpressions;

                model.ID = NHibernateHelper.CurrentSession.Save(model).ToString();
                NHibernateHelper.CurrentSession.Flush();

                return this.Create(model);
            }
            finally
            {
                this._lock.ReleaseWriterLock();
            }
        }

        private MetadataPermissionStrategy Create(MetadataPermissionStrategyModel model)
        {
            MetadataMember metadataMember = MetadataMember.Create(model.Member, this._cobject);
            if (metadataMember != null)
            {
                MetadataPermissionStrategy permission = new MetadataPermissionStrategy(model.ID, model.ObjectId, metadataMember, (MetadataPermissionValue)model.Value, MetadataExpressionSearcher.Parse(model.SearchExpressions, this._cobject));
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
                IList<MetadataPermissionStrategyModel> models = NHibernateHelper.CurrentSession.QueryOver<MetadataPermissionStrategyModel>().Where(x => x.ObjectId == this._cobject.ID).List();
                foreach (MetadataPermissionStrategyModel model in models)
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
