using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Crm.Api.Organization.Exceptions
{
    [Serializable]
    public class PositionParentCannotSelfException : OrganizationException
    {
        public PositionParentCannotSelfException()
        {
            this.ExceptionMessage = "上级不能设置为自己!";
        }

        public PositionParentCannotSelfException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {

        }
    }
}
