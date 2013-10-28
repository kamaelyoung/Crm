using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Coldew.Api.Workflow.Exceptions
{
    [Serializable]
    public class TianjiaZijiWeiXiajiZhiweiException : GongzuoliuException
    {
        public TianjiaZijiWeiXiajiZhiweiException()
        {
            this.ExceptionMessage = "不能将自己设置下级职位。";
        }

        public TianjiaZijiWeiXiajiZhiweiException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            
        }
    }
}
