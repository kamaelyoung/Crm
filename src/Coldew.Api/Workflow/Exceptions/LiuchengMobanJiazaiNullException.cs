using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Coldew.Api.Workflow.Exceptions
{
    [Serializable]
    public class LiuchengMobanJiazaiNullException : GongzuoliuException
    {
        public LiuchengMobanJiazaiNullException()
        {
            this.ExceptionMessage = "流程加载不能返回null";
        }

        public LiuchengMobanJiazaiNullException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            
        }
    }
}
