using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm.Api.Organization
{
    [Serializable]
    public class PositionCreateInfo
    {
        /// <summary>
        /// 上级职位
        /// </summary>
        public virtual string ParentId { get; set; }

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
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
    }
}
