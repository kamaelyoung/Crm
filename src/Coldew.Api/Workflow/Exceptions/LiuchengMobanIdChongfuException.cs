using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Coldew.Api.Workflow.Exceptions
{
    [Serializable]
    public class LiuchengMobanIdChongfuException : GongzuoliuException
    {
        public LiuchengMobanIdChongfuException()
        {
            this.ExceptionMessage = "流程id重复";
        }

        public LiuchengMobanIdChongfuException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            
        }
    }
}
