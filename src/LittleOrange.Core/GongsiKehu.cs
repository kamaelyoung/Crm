using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core.Organization;
using Newtonsoft.Json;
using Coldew.Core;
using LittleOrange.Data;
using Coldew.Core.DataServices;

namespace LittleOrange.Core
{
    public class GongsiKehu : Metadata
    {
        public GongsiKehu(string id, List<MetadataProperty> propertys, ColdewObject cobject, MetadataDataService dataService)
            : base(id, propertys, cobject, dataService)
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
