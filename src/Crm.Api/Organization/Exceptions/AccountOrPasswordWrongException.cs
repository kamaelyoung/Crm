using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Crm.Api.Organization.Exceptions
{
    [Serializable]
    public class AccountOrPasswordWrongException : OrganizationException
    {
        public AccountOrPasswordWrongException()
        {
            this.ExceptionMessage = "用户名或密码错误!";
        }

        public AccountOrPasswordWrongException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            
        }

        public override string Message
        {
            get
            {
                if (this.StringResouceProvider != null)
                {
                    return this.StringResouceProvider.GetString("ex_accountOrPasswordWrong");
                }
                return base.Message;
            }
        }
    }
}
