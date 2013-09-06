using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Crm.Api.Organization.Exceptions
{
    [Serializable]
    public class PasswordExpiredException : OrganizationException
    {
        public PasswordExpiredException()
        {
            this.ExceptionMessage = "密码已过期!";
        }

        public PasswordExpiredException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {

        }

        public override string Message
        {
            get
            {
                if (this.StringResouceProvider != null)
                {
                    return this.StringResouceProvider.GetString("ex_passwordExpired");
                }
                return base.Message;
            }
        }
    }
}
