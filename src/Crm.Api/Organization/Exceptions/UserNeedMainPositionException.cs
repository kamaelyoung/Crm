using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.Serialization;

namespace Crm.Api.Organization.Exceptions
{
    [Serializable]
    public class UserNeedMainPositionException : OrganizationException
    {
        public UserNeedMainPositionException()
        {
            this.ExceptionMessage = "必须指定主职位!";
        }

        public UserNeedMainPositionException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            
        }

        public override string Message
        {
            get
            {
                if (this.StringResouceProvider != null)
                {
                    return this.StringResouceProvider.GetString("ex_UserNeedMainPosition");
                }
                return base.Message;
            }
        }
    }
}
