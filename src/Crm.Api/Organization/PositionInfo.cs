using System;
using System.Collections.Generic;

using System.Text;


namespace Crm.Api.Organization
{
    [Serializable]
    public class PositionInfo
    {

        /// <summary>
        /// ID
        /// </summary>
        public virtual string ID { get; set; }

        /// <summary>
        /// 上级职位
        /// </summary>
        public virtual string ParentId { get; set; }

        /// <summary>
        /// 上级职位IdentityId
        /// </summary>
        public virtual int ParentIdentityId { get; set; }

        /// <summary>
        /// 职位名称
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
        /// 职位类型
        /// </summary>
        public virtual OrganizationType PositionType { get; set; }

        /// <summary>
        /// 自定义排序
        /// </summary>
        public virtual int Sort { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }

        /// <summary>
        /// 部门编号
        /// </summary>
        public virtual string DepartmentId { set; get; }

        /// <summary>
        /// 是否有子职位
        /// </summary>
        public virtual bool HaveChildren { set; get; }
    }
}
