using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Crm.Api.Exceptions
{
    [Serializable]
    public class ContactCustomerNullException: CrmException
    {
        public ContactCustomerNullException()
        {
            this.ExceptionMessage = "没有指定客户!";
        }

        public ContactCustomerNullException(string message)
        {
            this.ExceptionMessage = message;
        }

        public ContactCustomerNullException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            this.ExceptionMessage = info.GetString("ExceptionMessage");
        }
    }
}
