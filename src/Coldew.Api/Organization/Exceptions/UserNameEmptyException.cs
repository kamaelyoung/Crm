using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Coldew.Api.Organization.Exceptions
{
    [Serializable]
    public class UserNameEmptyException : OrganizationException
    {
        public UserNameEmptyException()
        {
            this.ExceptionMessage = "姓名不能为空!";
        }

        public UserNameEmptyException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            
        }

        public override string Message
        {
            get
            {
                if (this.StringResouceProvider != null)
                {
                    return this.StringResouceProvider.GetString("ex_userNameEmpty");
                }
                return base.Message;
            }
        }
    }
}
