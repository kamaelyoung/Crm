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
    public class ObjectPermissionManager
    {
        List<ObjectPermission> _permissions;
        protected ReaderWriterLock _lock;
        ColdewObject _coldewObject;

        public ObjectPermissionManager(ColdewObject coldewObject)
        {
            this._coldewObject = coldewObject;
            this._permissions = new List<ObjectPermission>();
            this._lock = new ReaderWriterLock();
        }

        public ObjectPermissionValue GetPermission(User user)
        {
            this._lock.AcquireReaderLock(0);
            try
            {
                int value = 0;
                foreach (ObjectPermission permission in this._permissions)
                {
                    if (permission.Member.Contains(user))
                    {
                        value = value | (int)permission.Value;
                    }
                }
                return (ObjectPermissionValue)value;
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public bool HasValue(User user, ObjectPermissionValue value)
        {
            this._lock.AcquireReaderLock(0);
            try
            {
                foreach (ObjectPermission permission in this._permissions)
                {
                    if (permission.Member.Contains(user) && permission.Value.HasFlag(value))
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

        public ObjectPermission Create(Member member, ObjectPermissionValue value)
        {
            this._lock.AcquireWriterLock(0);
            try
            {
                ObjectPermissionModel model = new ObjectPermissionModel();
                model.ObjectId = this._coldewObject.ID;
                model.MemberId = member.ID;
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

        private ObjectPermission Create(ObjectPermissionModel model)
        {
            Member member = this._coldewObject.ColdewManager.OrgManager.GetMember(model.MemberId);
            if (member != null)
            {
                ObjectPermission permission = new ObjectPermission(model.ID, member, model.ObjectId, (ObjectPermissionValue)model.Value);
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
                IList<ObjectPermissionModel> models = NHibernateHelper.CurrentSession.QueryOver<ObjectPermissionModel>().Where(x => x.ObjectId == this._coldewObject.ID).List();
                foreach (ObjectPermissionModel model in models)
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
