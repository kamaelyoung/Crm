using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.Serialization;

namespace Crm.Api.Organization.Exceptions
{
    [Serializable]
    public class AccountReapeatException : OrganizationException
    {
        public AccountReapeatException()
        {
            this.ExceptionMessage = "用户账号重复!";
        }

        public AccountReapeatException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            
        }

        public override string Message
        {
            get
            {
                if (this.StringResouceProvider != null)
                {
                    return this.StringResouceProvider.GetString("ex_accountReapeat");
                }
                return base.Message;
            }
        }
    }
}
