using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Coldew.Api.Organization.Exceptions
{
    [Serializable]
    public class AccountExpiredException : OrganizationException
    {
        public AccountExpiredException()
        {
            this.ExceptionMessage = "账户已失效!";
        }

        public AccountExpiredException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            
        }

        public override string Message
        {
            get
            {
                if (this.StringResouceProvider != null)
                {
                    return this.StringResouceProvider.GetString("ex_accountExpired");
                }
                return base.Message;
            }
        }
    }
}
