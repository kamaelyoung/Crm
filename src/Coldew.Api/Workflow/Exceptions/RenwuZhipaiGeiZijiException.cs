using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Coldew.Api.Workflow.Exceptions
{
    [Serializable]
    public class RenwuZhipaiGeiZijiException : GongzuoliuException
    {
        public RenwuZhipaiGeiZijiException()
        {
            this.ExceptionMessage = "不能将任务指派给自己";
        }

        public RenwuZhipaiGeiZijiException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            
        }
    }
}
