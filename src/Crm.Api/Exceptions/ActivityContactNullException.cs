using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Crm.Api.Exceptions
{
    [Serializable]
    public class ActivityContactNullException: CrmException
    {
        public ActivityContactNullException()
        {
            this.ExceptionMessage = "没有指定联系人!";
        }

        public ActivityContactNullException(string message)
        {
            this.ExceptionMessage = message;
        }

        public ActivityContactNullException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            this.ExceptionMessage = info.GetString("ExceptionMessage");
        }
    }
}
