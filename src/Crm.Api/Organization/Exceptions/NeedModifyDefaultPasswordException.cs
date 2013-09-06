using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.Serialization;

namespace Crm.Api.Organization.Exceptions
{
    [Serializable]
    public class NeedModifyDefaultPasswordException : OrganizationException
    {
        public NeedModifyDefaultPasswordException()
        {
            this.ExceptionMessage = "需要先修改默认密码!";
        }

        public NeedModifyDefaultPasswordException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            
        }

        public override string Message
        {
            get
            {
                if (this.StringResouceProvider != null)
                {
                    return this.StringResouceProvider.GetString("ex_NeedModifyDefaultPassword");
                }
                return base.Message;
            }
        }
    }
}
