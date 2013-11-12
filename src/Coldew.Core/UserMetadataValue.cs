using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api;
using Coldew.Core.Organization;
using Newtonsoft.Json.Linq;

namespace Coldew.Core
{
    public class UserMetadataValue : MetadataValue
    {
        public UserMetadataValue(User value, UserField field)
            : base(value, field)
        {

        }

        public User User
        {
            get
            {
                return this.Value;
            }
        }

        public override JToken PersistenceValue
        {
            get 
            {
                if (this.User != null)
                {
                    return this.User.Account;
                }
                return "";
            }
        }

        public override string ShowValue
        {
            get 
            {
                if (this.User != null)
                {
                    return this.User.Name; 
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
            get { return this.User.Account; }
        }
    }
}
