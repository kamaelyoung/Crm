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
        Dictionary<string, List<MetadataPermissionStrategy>> _permissionDict;
        protected ReaderWriterLock _lock;
        OrganizationManagement _orgManager;
        ColdewManager _coldewManager;

        public MetadataPermissionStrategyManager(ColdewManager coldewManager)
        {
            this._permissionDict = new Dictionary<string,List<MetadataPermissionStrategy>>();
            this._orgManager = coldewManager.OrgManager;
            this._coldewManager = coldewManager;
            this._lock = new ReaderWriterLock();
        }

        public MetadataPermissionValue GetPermission(string objectId, User user, Metadata metadata)
        {
            this._lock.AcquireReaderLock(0);
            try
            {
                int value = 0;
                if (this._permissionDict.ContainsKey(objectId))
                {
                    List<MetadataPermissionStrategy> permissions = this._permissionDict[objectId];
                    foreach (MetadataPermissionStrategy permission in permissions)
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

        public bool HasValue(string objectId, User user, MetadataPermissionValue value, Metadata metadata)
        {
            this._lock.AcquireReaderLock(0);
            try
            {
                if (this._permissionDict.ContainsKey(objectId))
                {
                    List<MetadataPermissionStrategy> permissions = this._permissionDict[objectId];
                    foreach (MetadataPermissionStrategy permission in permissions)
                    {
                        if (permission.HasValue(metadata, user, value))
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

        public MetadataPermissionStrategy Create(string objectId, MetadataMember member, MetadataPermissionValue value, string searchExpressions)
        {
            this._lock.AcquireWriterLock(0);
            try
            {
                MetadataPermissionStrategyModel model = new MetadataPermissionStrategyModel();
                model.ObjectId = objectId;
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
            ColdewObject cobject = this._coldewManager.ObjectManager.GetFormById(model.ObjectId);
            if (cobject != null)
            {
                MetadataMember metadataMember = MetadataMember.Create(model.Member, this._orgManager);
                if (metadataMember != null)
                {
                    MetadataPermissionStrategy permission = new MetadataPermissionStrategy(model.ID, model.ObjectId, metadataMember, (MetadataPermissionValue)model.Value, MetadataExpressionSearcher.Parse(model.SearchExpressions, cobject));

                    if (!this._permissionDict.ContainsKey(model.ObjectId))
                    {
                        this._permissionDict.Add(model.ObjectId, new List<MetadataPermissionStrategy>());
                    }
                    this._permissionDict[model.ObjectId].Add(permission);
                    return permission;
                }
            } 
            return null;
        }

        internal void Load()
        {
            this._lock.AcquireWriterLock(0);
            try
            {
                IList<MetadataPermissionStrategyModel> models = NHibernateHelper.CurrentSession.QueryOver<MetadataPermissionStrategyModel>().List();
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
