using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.Serialization;

namespace Coldew.Api.Organization.Exceptions
{
    [Serializable]
    public class LeaderPositionCannotSelfException : OrganizationException
    {
        public LeaderPositionCannotSelfException()
        {
            this.ExceptionMessage = "其他上级不能设置为自己!";
        }

        public LeaderPositionCannotSelfException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {

        }

        public override string Message
        {
            get
            {
                if (this.StringResouceProvider != null)
                {
                    return this.StringResouceProvider.GetString("ex_LeaderPositionCannotSelf");
                }
                return base.Message;
            }
        }
    }
}
