using System;
using System.Collections.Generic;

using System.Text;

namespace Crm.Api.Organization
{
    [Serializable]
    public class GroupInfo
    {
        /// <summary>
        /// ID
        /// </summary>
        public virtual string ID { get; set; }

        public virtual GroupType GroupType { get; set; }

        /// <summary>
        /// 组名称
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
        /// 是否个人用户组
        /// </summary>
        public virtual bool IsPersonal { get; set; }

        public virtual bool IsSystem { get; set; }

        public virtual DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public virtual string CreatorId { get; set; }

        public virtual string CreatorName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
    }
}
