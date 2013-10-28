using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Coldew.Api.Workflow.Exceptions
{
    [Serializable]
    public class ChongfuTianjiaYonghuException : GongzuoliuException
    {
        public ChongfuTianjiaYonghuException()
        {
            this.ExceptionMessage = "重复添加用户。";
        }

        public ChongfuTianjiaYonghuException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            
        }
    }
}
