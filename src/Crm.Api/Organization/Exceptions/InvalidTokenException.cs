using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.Serialization;

namespace Crm.Api.Organization.Exceptions
{
    [Serializable]
    public class InvalidTokenException:OrganizationException
    {
        public InvalidTokenException()
        {
            this.ExceptionMessage = "Token无效!";
        }

        public InvalidTokenException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {

        }

        public override string Message
        {
            get
            {
                if (this.StringResouceProvider != null)
                {
                    return this.StringResouceProvider.GetString("ex_InvalidToken");
                }
                return base.Message;
            }
        }
    }
}
