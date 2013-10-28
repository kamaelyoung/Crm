using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Coldew.Api.Workflow.Exceptions
{
    [Serializable]
    public class RenwuChongfuZhipaiException : GongzuoliuException
    {
        public RenwuChongfuZhipaiException()
        {
            this.ExceptionMessage = "任务重复指派";
        }

        public RenwuChongfuZhipaiException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            
        }
    }
}
