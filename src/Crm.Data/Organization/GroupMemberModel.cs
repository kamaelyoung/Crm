using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm.Data.Organization
{
    /// <summary>
    /// 用户组成员
    /// </summary>
    public class GroupMemberModel
    {
        public GroupMemberModel()
        {
        }

        /// <summary>
        /// ID
        /// </summary>
        public virtual int ID { get; set; }

        /// <summary>
        /// 用户组ID
        /// </summary>
        public virtual string GroupId { get; set; }

        /// <summary>
        /// 成员ID
        /// </summary>
        public virtual string MemberId { get; set; }

        /// <summary>
        /// 成员类型
        /// </summary>
        public virtual int MemberType { get; set; }
    }
}
