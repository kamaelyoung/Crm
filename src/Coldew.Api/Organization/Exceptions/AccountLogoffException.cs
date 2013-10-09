using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.Serialization;

namespace Coldew.Api.Organization.Exceptions
{
    [Serializable]
    public class AccountLogoffException : OrganizationException
    {
        public AccountLogoffException()
        {
            this.ExceptionMessage = "账号已经被注销!";
        }

        public AccountLogoffException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            
        }

        public override string Message
        {
            get
            {
                if (this.StringResouceProvider != null)
                {
                    return this.StringResouceProvider.GetString("ex_accountLogoffed");
                }
                return base.Message;
            }
        }
    }
}
