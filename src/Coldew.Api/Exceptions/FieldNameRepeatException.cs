using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Coldew.Api.Exceptions
{
    [Serializable]
    public class FieldNameRepeatException : ColdewException
    {
        public FieldNameRepeatException()
        {
            this.ExceptionMessage = "字段名称重复!";
        }

        public FieldNameRepeatException(string message)
        {
            this.ExceptionMessage = message;
        }

        public FieldNameRepeatException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            this.ExceptionMessage = info.GetString("ExceptionMessage");
        }
    }
}
