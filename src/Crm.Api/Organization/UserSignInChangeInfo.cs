using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm.Api.Organization
{
    public class UserSignInChangeInfo
    {
        public UserSignInChangeInfo(UserInfo userInfo)
        {
            this.ID = userInfo.ID;
            this.LastLoginTime = userInfo.LastLoginTime;
            this.LastLoginIp = userInfo.LastLoginIp;
        }

        /// <summary>
        /// ID
        /// </summary>
        public virtual string ID { get; set; }

        /// <summary>
        /// 最后一次登录时间
        /// </summary>
        public virtual DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 最后一次登录IP
        /// </summary>
        public virtual string LastLoginIp { get; set; }
    }
}
