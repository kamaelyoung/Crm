using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core.Organization;

namespace Coldew.Core
{
    public class MetadataFieldMember : MetadataMember
    {
        string _fieldCode;
        public MetadataFieldMember(string fieldCode)
        {
            this._fieldCode = fieldCode;
        }

        public override bool Contains(Metadata metadata, User user)
        {
            MetadataProperty property = metadata.GetProperty(this._fieldCode);
            if (property != null)
            {
                if (property.Value is UserMetadataValue)
                {
                    UserMetadataValue userValue = property.Value as UserMetadataValue;
                    return userValue.User == user;
                }
                else if (property.Value is UserListMetadataValue)
                {
                    UserListMetadataValue userListValue = property.Value as UserListMetadataValue;
                    return userListValue.Users.Contains(user);
                }
            }
            return false;
        }

        public override string Serialize()
        {
            return "field:" + this._fieldCode;
        }
    }
}
