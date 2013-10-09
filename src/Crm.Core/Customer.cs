using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core.Organization;
using Crm.Data;
using Crm.Api;
using Crm.Api.Exceptions;
using Newtonsoft.Json;
using Coldew.Core;

namespace Crm.Core
{
    public class Customer : Metadata
    {
        public Customer(string id, MetadataPropertyList propertys, Form form)
            : base(id, propertys, form)
        {

        }

        private CustomerAreaMetadataValue AreaValue
        {
            get
            {
                return this.GetProperty(CrmFormConstCode.CUST_FIELD_NAME_AREA).Value as CustomerAreaMetadataValue;
            }
        }

        public CustomerArea Area { get { return this.AreaValue.Area; } }

        private UserListMetadataValue SalesUsersValue
        {
            get
            {
                return this.GetProperty(CrmFormConstCode.CUST_FIELD_NAME_SALES_USERS).Value as UserListMetadataValue;
            }
        }

        public List<User> SalesUsers { get { return this.SalesUsersValue.Users; } }

        protected override void UpdateDB(MetadataPropertyList propertys)
        {
            CustomerModel model = NHibernateHelper.CurrentSession.Get<CustomerModel>(this.ID);
            model.PropertysJson = propertys.ToJson();

            NHibernateHelper.CurrentSession.Update(model);
        }

        protected override void DeleteDB()
        {
            CustomerModel model = NHibernateHelper.CurrentSession.Get<CustomerModel>(this.ID);

            NHibernateHelper.CurrentSession.Delete(model);
            NHibernateHelper.CurrentSession.Flush();
        }

        public override bool CanPreview(User user)
        {
            bool canPreview = base.CanPreview(user);
            if (canPreview)
            {
                return true;
            }

            if (this.SalesUsers.Contains(user))
            {
                return true;
            }

            if (this.Area.ManagerUsers.Contains(user))
            {
                return true;
            }

            if (this.SalesUsers.Any(x => x.IsMySuperior(user, true)))
            {
                return true;
            }

            if (this.Area.ManagerUsers.Any(x => x.IsMySuperior(user, true)))
            {
                return true;
            }

            return false;
        }

        public override bool CanDelete(User user)
        {
            bool canDelete = base.CanPreview(user);
            if (canDelete)
            {
                return true;
            }

            if (this.Area.ManagerUsers.Contains(user))
            {
                return true;
            }

            if (this.Area.ManagerUsers.Any(x => x.IsMySuperior(user, true)))
            {
                return true;
            }

            return false;
        }
    }
}
