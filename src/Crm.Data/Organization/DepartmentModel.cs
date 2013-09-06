using System;
using System.Collections.Generic;


namespace Crm.Data.Organization
{
    /// <summary>
    /// 部门
    /// </summary>
    public class DepartmentModel
    {
        public DepartmentModel()
        {

        }

        /// <summary>
        /// ID
        /// </summary>
        public virtual string ID { get; set; }

        public virtual string ParentId { get; set; }

        /// <summary>
        /// 部门主管职位ID
        /// </summary>
        public virtual string ManagerPositionId { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
    }
}
