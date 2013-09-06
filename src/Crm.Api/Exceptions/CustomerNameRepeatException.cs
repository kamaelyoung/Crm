using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Crm.Api.Exceptions
{
    [Serializable]
    public class CustomerNameRepeatException: CrmException
    {
        public CustomerNameRepeatException()
        {
            this.ExceptionMessage = "客户名称重复!";
        }

        public CustomerNameRepeatException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            this.ExceptionMessage = info.GetString("ExceptionMessage");
        }
    }
}
