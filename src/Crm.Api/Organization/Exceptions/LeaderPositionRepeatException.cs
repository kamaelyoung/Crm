using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.Serialization;

namespace Crm.Api.Organization.Exceptions
{
    [Serializable]
    public class LeaderPositionRepeatException : OrganizationException
    {
        public LeaderPositionRepeatException()
        {
            this.ExceptionMessage = "其他上级重复!";
        }

        public LeaderPositionRepeatException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {

        }

        public override string Message
        {
            get
            {
                if (this.StringResouceProvider != null)
                {
                    return this.StringResouceProvider.GetString("ex_LeaderPositionRepeat");
                }
                return base.Message;
            }
        }
    }
}
