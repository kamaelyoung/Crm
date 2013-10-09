using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Coldew.Api.Organization.Exceptions
{
    [Serializable]
    public class AccountEmptyException : OrganizationException
    {
        public AccountEmptyException()
        {
            this.ExceptionMessage = "账号不能为空!";
        }

        public AccountEmptyException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            
        }

        public override string Message
        {
            get
            {
                if (this.StringResouceProvider != null)
                {
                    return this.StringResouceProvider.GetString("ex_accountEmpty");
                }
                return base.Message;
            }
        }
    }
}
