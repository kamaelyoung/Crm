using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.Serialization;

namespace Coldew.Api.Organization.Exceptions
{
    [Serializable]
    public class PasswordComplexInvalidException : OrganizationException 
    {
        public PasswordComplexInvalidException()
        {
            this.ExceptionMessage = "密码要求必须包含以下三类字符中的两类字符：(A 到 Z)、(a 到 z)、(0 到 9)";
        }

        public PasswordComplexInvalidException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            
        }

        public override string Message
        {
            get
            {
                if (this.StringResouceProvider != null)
                {
                    return this.StringResouceProvider.GetString("ex_PasswordComplexInvalid");
                }
                return base.Message;
            }
        }
    }
}
