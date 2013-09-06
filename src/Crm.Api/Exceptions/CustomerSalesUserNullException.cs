using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Crm.Api.Exceptions
{
    [Serializable]
    public class CustomerSalesUserNullException: CrmException
    {
        public CustomerSalesUserNullException()
        {
            this.ExceptionMessage = "没有指定销售员!";
        }

        public CustomerSalesUserNullException(string message)
        {
            this.ExceptionMessage = message;
        }

        public CustomerSalesUserNullException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            this.ExceptionMessage = info.GetString("ExceptionMessage");
        }
    }
}
