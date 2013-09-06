using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.Serialization;

namespace Crm.Api.Organization.Exceptions
{
    [Serializable]
    public class AccountNotFoundException : OrganizationException
    {

        public AccountNotFoundException()
        {
            this.ExceptionMessage = "账号未找到!";
        }

        public AccountNotFoundException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            
        }

        public override string Message
        {
            get
            {
                if (this.StringResouceProvider != null)
                {
                    return this.StringResouceProvider.GetString("ex_accountNotFound");
                }
                return base.Message;
            }
        }
    }
}
