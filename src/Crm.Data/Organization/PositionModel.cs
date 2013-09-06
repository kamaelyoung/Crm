using System;
using System.Collections.Generic;

namespace Crm.Data.Organization
{
    /// <summary>
    /// 职位
    /// </summary>
    public class PositionModel
    {

        public PositionModel()
        {

        }

        /// <summary>
        /// ID
        /// </summary>
        public virtual string ID { get; set; }

        /// <summary>
        /// 上级职位
        /// </summary>
        public virtual string ParentId { get; set; }

        /// <summary>
        /// 职位名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
    }
}
