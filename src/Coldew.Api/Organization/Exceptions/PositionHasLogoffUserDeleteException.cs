using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.Serialization;

namespace Coldew.Api.Organization.Exceptions
{
    [Serializable]
    public class PositionHasLogoffUserDeleteException : OrganizationException
    {
        public PositionHasLogoffUserDeleteException()
        {
            this.ExceptionMessage = "无法删除职位，职位中包含有已注销用户!";
        }

        public PositionHasLogoffUserDeleteException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            
        }

        public override string Message
        {
            get
            {
                if (this.StringResouceProvider != null)
                {
                    return this.StringResouceProvider.GetString("ex_PositionHasLogoffUserDelete");
                }
                return base.Message;
            }
        }
    }
}
