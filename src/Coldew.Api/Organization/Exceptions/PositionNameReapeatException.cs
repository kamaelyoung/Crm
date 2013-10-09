using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.Serialization;

namespace Coldew.Api.Organization.Exceptions
{
    [Serializable]
    public class PositionNameReapeatException : OrganizationException
    {
        public PositionNameReapeatException()
        {
            this.ExceptionMessage = "职位名称重复!";
        }

        public PositionNameReapeatException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            
        }

        public override string Message
        {
            get
            {
                if (this.StringResouceProvider != null)
                {
                    return this.StringResouceProvider.GetString("ex_PositionNameReapeat");
                }
                return base.Message;
            }
        }
    }
}
