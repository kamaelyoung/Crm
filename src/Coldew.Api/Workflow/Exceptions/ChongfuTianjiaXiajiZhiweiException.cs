using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Coldew.Api.Workflow.Exceptions
{
    [Serializable]
    public class ChongfuTianjiaXiajiZhiweiException : GongzuoliuException
    {
        public ChongfuTianjiaXiajiZhiweiException()
        {
            this.ExceptionMessage = "重复添加下级职位。";
        }

        public ChongfuTianjiaXiajiZhiweiException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            
        }
    }
}
