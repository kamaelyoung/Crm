using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.Serialization;

namespace Crm.Api.Organization.Exceptions
{
    [Serializable]
    public class NotDownRmoveException : OrganizationException
    {
        public NotDownRmoveException()
        {
            this.ExceptionMessage = "没有下级元素，无法移动!";
        }

        public NotDownRmoveException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {

        }

        public override string Message
        {
            get
            {
                if (this.StringResouceProvider != null)
                {
                    return this.StringResouceProvider.GetString("ex_NotDownRmove");
                }
                return base.Message;
            }
        }
    }
}
