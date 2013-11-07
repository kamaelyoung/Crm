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
using Coldew.Core.DataServices;

namespace Crm.Core
{
    public class Customer : Metadata
    {
        public Customer(string id, List<MetadataProperty> propertys, ColdewObject form, MetadataDataService dataService)
            : base(id, propertys, form, dataService)
        {

        }

        private CustomerAreaMetadataValue AreaValue
        {
            get
            {
                return this.GetProperty(CrmObjectConstCode.CUST_FIELD_NAME_AREA).Value as CustomerAreaMetadataValue;
            }
        }

        public CustomerArea Area { get { return this.AreaValue.Area; } }

        private UserListMetadataValue SalesUsersValue
        {
            get
            {
                return this.GetProperty(CrmObjectConstCode.CUST_FIELD_NAME_SALES_USERS).Value as UserListMetadataValue;
            }
        }

        public List<User> SalesUsers { get { return this.SalesUsersValue.Users; } }

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
