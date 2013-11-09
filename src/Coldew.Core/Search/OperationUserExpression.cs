using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api.Exceptions;

namespace Coldew.Core.Search
{
    public class OperationUserExpression : SearchExpression
    {
        public OperationUserExpression(Field field)
            : base(field)
        {

        }

        public override bool Compare(Organization.User opUser, Metadata metadata)
        {
            MetadataProperty property = metadata.GetProperty(this.Field.Code);
            if (property != null)
            {
                if (!(property.Value is UserMetadataValue) && !(property.Value is UserListMetadataValue))
                {
                    throw new ColdewException(string.Format("{0} 不是用户类型字段, 无法执行搜索", this.Field.Name));
                }

                if (property.Value is UserMetadataValue)
                {
                    UserMetadataValue userMetadataValue = property.Value as UserMetadataValue;
                    return userMetadataValue.User == opUser;
                }
                else
                {
                    UserListMetadataValue userListMetadataValue = property.Value as UserListMetadataValue;
                    return userListMetadataValue.Users.Contains(opUser);
                }
            }

            return true;
        }
    }
}
