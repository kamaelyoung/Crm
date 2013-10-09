using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api;
using Coldew.Core.Organization;

namespace Coldew.Core
{
    public class UserListField : Field
    {
        UserManagement _userManager;
        public UserListField(FieldNewInfo info, bool defaultValueIsCurrent, UserManagement userManager)
            :base(info)
        {
            this.DefaultValueIsCurrent = defaultValueIsCurrent;
            this._userManager = userManager;
        }

        public override string Type
        {
            get { return FieldType.UserList; }
        }

        public override string TypeName
        {
            get { return "用户"; }
        }

        public bool DefaultValueIsCurrent { set; get; }

        public override MetadataValue CreateMetadataValue(string value)
        {
            List<User> users = new List<User>();
            if (!string.IsNullOrEmpty(value))
            {
                foreach (string account in value.Split(','))
                {
                    users.Add(this._userManager.GetUserByAccount(account));
                }
            }
            return new UserListMetadataValue(users, this);
        }

        public override FieldInfo Map()
        {
            UserListFieldInfo info = new UserListFieldInfo();
            this.Fill(info);
            info.DefaultValueIsCurrent = this.DefaultValueIsCurrent;
            return info;
        }
    }
}
