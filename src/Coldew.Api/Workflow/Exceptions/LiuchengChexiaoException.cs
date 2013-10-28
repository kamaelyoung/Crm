using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Coldew.Api.Workflow.Exceptions
{
    [Serializable]
    public class LiuchengChexiaoException : GongzuoliuException
    {
        public LiuchengChexiaoException()
        {
            this.ExceptionMessage = "该流程正在审核无法撤销, 需要撤销请通知审核人退回。";
        }

        public LiuchengChexiaoException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            
        }
    }
}
