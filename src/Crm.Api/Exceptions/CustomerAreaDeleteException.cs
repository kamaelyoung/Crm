using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Crm.Api.Exceptions
{
    [Serializable]
    public class CustomerAreaDeleteException: CrmException
    {
        public CustomerAreaDeleteException()
        {
            this.ExceptionMessage = "无法删除该区域!";
        }

        public CustomerAreaDeleteException(string message)
        {
            this.ExceptionMessage = message;
        }

        public CustomerAreaDeleteException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            this.ExceptionMessage = info.GetString("ExceptionMessage");
        }
    }
}
