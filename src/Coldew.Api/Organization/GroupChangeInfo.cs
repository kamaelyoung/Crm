using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Api.Organization
{
    [Serializable]
    public class GroupChangeInfo
    {
        public GroupChangeInfo(GroupInfo groupInfo)
        {
            this.ID = groupInfo.ID;
            this.Name = groupInfo.Name;
            this.Remark = groupInfo.Remark;
        }

        /// <summary>
        /// ID
        /// </summary>
        public virtual string ID { get; set; }

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
