using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Crm.Api.Organization.Exceptions
{
    [Serializable]
    public class CannotChangeSystemGroupCodeException : OrganizationException
    {
        public CannotChangeSystemGroupCodeException()
        {
            this.ExceptionMessage = "不能修改系统用户组的编号!";
        }

        public CannotChangeSystemGroupCodeException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            
        }

        public override string Message
        {
            get
            {
                if (this.StringResouceProvider != null)
                {
                    return this.StringResouceProvider.GetString("ex_cannotChangeSystemGroupCode");
                }
                return base.Message;
            }
        }
    }
}
