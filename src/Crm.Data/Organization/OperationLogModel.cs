using System;

namespace Crm.Data.Organization
{
    /// <summary>
    /// 操作日志
    /// </summary>
    [Serializable]
    public class OperationLogModel
    {
        public OperationLogModel()
        {

        }

        /// <summary>
        /// ID
        /// </summary>
        public virtual long ID { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public virtual int OperationType { get; set; }

        /// <summary>
        /// 操作内容
        /// </summary>
        public virtual string OperationContent { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public virtual DateTime OperationTime { get; set; }

        /// <summary>
        /// 操作人ID
        /// </summary>
        public virtual string OperatorId { get; set; }

        /// <summary>
        /// 操作人姓名
        /// </summary>
        public virtual string OperatorName { get; set; }
    }
}
