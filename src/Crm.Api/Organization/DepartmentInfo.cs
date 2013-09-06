using System;
using System.Collections.Generic;

using System.Text;


namespace Crm.Api.Organization
{
    [Serializable]
    public class DepartmentInfo
    {
        /// <summary>
        /// ID
        /// </summary>
        public virtual string ID { get; set; }

        /// <summary>
        /// 上级部门id
        /// </summary>
        public virtual string ParentId { set; get; }

        /// <summary>
        /// 部门主管职位
        /// </summary>
        public virtual string ManagerPositionId { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        string _name;
        public virtual string Name 
        {
            get
            {
                return _name;
            }
            set
            {
                if (value != null)
                {
                    value = value.Trim();
                }
                _name = value;
            }
        }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
    }
}
