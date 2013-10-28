using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core.Organization;
using Newtonsoft.Json;
using Coldew.Core;

namespace LittleOrange.Core
{
    public class Customer : Metadata
    {
        public Customer(string id, List<MetadataProperty> propertys, ColdewObject form)
            : base(id, propertys, form)
        {

        }

        private UserListMetadataValue SalesUsersValue
        {
            get
            {
                return this.GetProperty(LittleOrangeObjectConstCode.CUST_FIELD_NAME_SALES_USERS).Value as UserListMetadataValue;
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

            if (this.SalesUsers.Any(x => x.IsMySuperior(user, true)))
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

            return false;
        }
    }
}
