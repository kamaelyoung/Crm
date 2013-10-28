using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Coldew.Api.Workflow.Exceptions
{
    [Serializable]
    public class RenwuChongfuChuliException : GongzuoliuException
    {
        public RenwuChongfuChuliException(string chuliren, DateTime chulirenShijian)
        {
            this.ExceptionMessage = string.Format("任务已经被{0}在{1}处理了", chuliren, chulirenShijian);
        }

        public RenwuChongfuChuliException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            
        }
    }
}
