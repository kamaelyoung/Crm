using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.Serialization;

namespace Coldew.Api.Organization.Exceptions
{
    [Serializable]
    public class PositionHasUserDeleteException : OrganizationException
    {
        public PositionHasUserDeleteException()
        {
            this.ExceptionMessage = "无法删除职位，职位中包含有用户!";
        }

        public PositionHasUserDeleteException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            
        }

        public override string Message
        {
            get
            {
                if (this.StringResouceProvider != null)
                {
                    return this.StringResouceProvider.GetString("ex_PositionHasUserDelete");
                }
                return base.Message;
            }
        }
    }
}
