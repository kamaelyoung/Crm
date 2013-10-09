using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Coldew.Api.Organization.Exceptions
{
    [Serializable]
    public class AccountSignedException : OrganizationException
    {
        public AccountSignedException()
        {
            this.ExceptionMessage = "账号已经登录!";
        }

        public AccountSignedException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            
        }

        public override string Message
        {
            get
            {
                if (this.StringResouceProvider != null)
                {
                    return this.StringResouceProvider.GetString("ex_accountSigned");
                }
                return base.Message;
            }
        }
    }
}
