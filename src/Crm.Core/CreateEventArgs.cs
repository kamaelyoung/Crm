using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm.Core
{
    public class CreateEventArgs<CreateInfoT, SnapshotInfoT, CreatedObjectT> : OperationEventArgs
    {
        /// <summary>
        /// 创建信息
        /// </summary>
        public CreateInfoT CreateInfo { set; get; }

        /// <summary>
        /// 创建以后的对象
        /// </summary>
        public CreatedObjectT CreatedObject { get; set; }

        /// <summary>
        /// 创建以后对象快照
        /// </summary>
        public SnapshotInfoT CreatedSnapshotInfo { set; get; }
    }
}
