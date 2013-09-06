using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Crm.Api.Exceptions
{
    [Serializable]
    public class CrmException : ApplicationException
    {
        public CrmException()
        {
            this.ExceptionMessage = "操作异常!";
        }

        public CrmException(string message)
        {
            this.ExceptionMessage = message;
        }

        public CrmException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            this.ExceptionMessage = info.GetString("ExceptionMessage");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("ExceptionMessage", this.ExceptionMessage);
        }

        protected string ExceptionMessage { set; get; }

        public override string Message
        {
            get
            {
                return this.ExceptionMessage;
            }
        }
    }
}
