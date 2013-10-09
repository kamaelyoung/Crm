using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Core
{
    public class DeleteEventArgs<DeleteObjectT> : OperationEventArgs
    {
        /// <summary>
        /// 删除的对象
        /// </summary>
        public DeleteObjectT DeleteObject { set; get; }
    }
}
