using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api;
using Coldew.Core.Organization;

namespace Coldew.Core
{
    public class UserField : Field
    {
        UserManagement _userManager;
        public UserField(FieldNewInfo info, bool defaultValueIsCurrent, UserManagement userManager)
            :base(info)
        {
            this.DefaultValueIsCurrent = defaultValueIsCurrent;
            this._userManager = userManager;
        }

        public override string Type
        {
            get { return FieldType.User; }
        }

        public override string TypeName
        {
            get { return "用户"; }
        }

        public bool DefaultValueIsCurrent { set; get; }

        public override MetadataValue CreateMetadataValue(string value)
        {
            User user = null;
            if (!string.IsNullOrEmpty(value))
            {
                user = this._userManager.GetUserByAccount(value);
            }
            return new UserMetadataValue(user, this);
        }

        public MetadataValue CreateMetadataValue(User value)
        {
            return new UserMetadataValue(value, this);
        }

        public override FieldInfo Map()
        {
            UserFieldInfo info = new UserFieldInfo();
            this.Fill(info);
            info.DefaultValueIsCurrent = this.DefaultValueIsCurrent;
            return info;
        }
    }
}
