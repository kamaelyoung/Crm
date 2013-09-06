using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.Serialization;

namespace Crm.Api.Organization.Exceptions
{
    [Serializable]
    public class NotUpRemoveException:OrganizationException
    {
        public NotUpRemoveException()
        {
            this.ExceptionMessage = "没有上级元素，无法移动!";
        }

        public NotUpRemoveException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {

        }

        public override string Message
        {
            get
            {
                if (this.StringResouceProvider != null)
                {
                    return this.StringResouceProvider.GetString("ex_NotUpRemove");
                }
                return base.Message;
            }
        }
    }
}
