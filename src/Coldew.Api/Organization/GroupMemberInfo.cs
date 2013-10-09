using System;
using System.Collections.Generic;

using System.Text;

namespace Coldew.Api.Organization
{
    [Serializable]
    public class GroupMemberInfo
    {
        /// <summary>
        /// 用户组ID
        /// </summary>
        public virtual string GroupId { get; set; }

        /// <summary>
        /// 成员ID
        /// </summary>
        public virtual string MemberId { get; set; }

        public virtual string MemberName { set; get; }

        /// <summary>
        /// 成员类型
        /// </summary>
        public virtual MemberType MemberType { get; set; }
    }
}
