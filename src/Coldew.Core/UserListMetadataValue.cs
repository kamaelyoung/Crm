using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core.Organization;
using Coldew.Api;

namespace Coldew.Core
{
    public class UserListMetadataValue : MetadataValue
    {
        public UserListMetadataValue(List<User> value, Field field)
            : base(value, field)
        {

        }

        public List<User> Users
        {
            get
            {
                return this.Value;
            }
        }

        public override string PersistenceValue
        {
            get 
            {
                if (this.Users != null)
                {
                    return string.Join(",", this.Users.Select(x => x.Account));
                }
                return "";
            }
        }

        public override string ShowValue
        {
            get 
            {
                if (this.Users != null)
                {
                    return string.Join(",", this.Users.Select(x => x.Name)); 
                }
                return "";
            }
        }

        public override dynamic OrderValue
        {
            get { return this.ShowValue; }
        }

        public override dynamic EditValue
        {
            get { return this.Users.Select(x => x.Account).ToArray(); }
        }
    }
}
