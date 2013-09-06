using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm.Api.Organization
{
    [Serializable]
    public class GroupCreateInfo
    {

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
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
    }
}
