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
    public class FieldPermissionManager
    {
        List<FieldPermission> _permissions;
        protected ReaderWriterLock _lock;
        OrganizationManagement _orgManager;
        ColdewObject _cobject;

        public FieldPermissionManager(ColdewObject cobject)
        {
            this._cobject = cobject;
            this._permissions = new List<FieldPermission>();
            this._orgManager = cobject.ColdewManager.OrgManager;
            this._lock = new ReaderWriterLock();
        }

        public FieldPermissionValue GetPermission(User user, Field field)
        {
            this._lock.AcquireReaderLock(0);
            try
            {
                List<FieldPermission> permissions = this._permissions.Where(x => x.FieldCode == field.Code).ToList();
                if (permissions.Count == 0)
                {
                    return FieldPermissionValue.All;
                }

                int value = 0;
                foreach (FieldPermission permission in permissions)
                {
                    if (permission.Member.Contains(user))
                    {
                        value = value | (int)permission.Value;
                    }
                }
                return (FieldPermissionValue)value;
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public bool HasValue(User user, FieldPermissionValue value, Field field)
        {
            this._lock.AcquireReaderLock(0);
            try
            {
                List<FieldPermission> permissions = this._permissions.Where(x => x.FieldCode == field.Code).ToList();
                if (permissions.Count == 0)
                {
                    return true;
                }

                foreach (FieldPermission permission in permissions)
                {
                    if (permission.HasValue(user, value))
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

        public FieldPermission Create(string fieldCode, Member member, FieldPermissionValue value)
        {
            this._lock.AcquireWriterLock(0);
            try
            {
                FieldPermissionModel model = new FieldPermissionModel();
                model.ObjectId = this._cobject.ID;
                model.FieldCode = fieldCode;
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

        private FieldPermission Create(FieldPermissionModel model)
        {
            Member member = this._orgManager.GetMember(model.MemberId);
            if (member != null)
            {
                FieldPermission permission = new FieldPermission(model.ID, model.FieldCode, member, (FieldPermissionValue)model.Value);
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
                IList<FieldPermissionModel> models = NHibernateHelper.CurrentSession.QueryOver<FieldPermissionModel>().Where(x => x.ObjectId == this._cobject.ID).List();
                foreach (FieldPermissionModel model in models)
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
