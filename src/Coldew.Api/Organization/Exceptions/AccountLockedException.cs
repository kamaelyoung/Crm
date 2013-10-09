using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.Serialization;

namespace Coldew.Api.Organization.Exceptions
{
    [Serializable]
    public class AccountLockedException : OrganizationException
    {
        public AccountLockedException()
        {
            this.ExceptionMessage = "账号已经被锁定!";
        }

        public AccountLockedException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            
        }
        
        public override string Message
        {
            get
            {
                if (this.StringResouceProvider != null)
                {
                    return this.StringResouceProvider.GetString("ex_accountLocked");
                }
                return base.Message;
            }
        }
    }
}
