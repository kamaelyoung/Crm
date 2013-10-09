using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Api.Organization
{
    [Serializable]
    public class DepartmentChangeInfo
    {
        public DepartmentChangeInfo(DepartmentInfo departmentInfo)
        {
            this.ID = departmentInfo.ID;
            this.Name = departmentInfo.Name;
            this.Remark = departmentInfo.Remark;
        }

        /// <summary>
        /// ID
        /// </summary>
        public virtual string ID { get; set; }

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
