using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Core
{
    /// <summary>
    /// 对象修改事件参数
    /// </summary>
    /// <typeparam name="ChangeInfoT"></typeparam>
    /// <typeparam name="SnapshotInfoT"></typeparam>
    /// <typeparam name="ChangeObjectT"></typeparam>
    public class ChangeEventArgs<ChangeInfoT, SnapshotInfoT, ChangeObjectT> : OperationEventArgs
    {
        /// <summary>
        /// 修改信息
        /// </summary>
        public ChangeInfoT ChangeInfo { set; get; }

        /// <summary>
        /// 修改以前的快照信息
        /// </summary>
        public SnapshotInfoT ChangingSnapshotInfo { set; get; }

        /// <summary>
        /// 修改以后的快照信息
        /// </summary>
        public SnapshotInfoT ChangedSnapshotInfo { set; get; }

        /// <summary>
        /// 修改的对象
        /// </summary>
        public ChangeObjectT ChangeObject { get; set; }
    }
}
