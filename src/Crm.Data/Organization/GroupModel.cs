using System;
using System.Collections.Generic;

namespace Crm.Data.Organization
{
    /// <summary>
    /// 用户组
    /// </summary>
    public class GroupModel
    {
        public GroupModel()
        {

        }

        /// <summary>
        /// ID
        /// </summary>
        public virtual string ID { get; set; }

        /// <summary>
        /// 用户组名称
        /// </summary>
        public virtual string Name { get; set; }

        public virtual int GroupType { get; set; }

        public virtual DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public virtual string CreatorId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
    }
}
