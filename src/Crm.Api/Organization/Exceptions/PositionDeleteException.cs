using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.Serialization;

namespace Crm.Api.Organization.Exceptions
{
    [Serializable]
    public class PositionDeleteException : OrganizationException
    {
        public PositionDeleteException(string message)
            :base(message)
        {
            
        }

        public PositionDeleteException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            
        }

        public override string Message
        {
            get
            {
                return base.Message;
            }
        }
    }
}
