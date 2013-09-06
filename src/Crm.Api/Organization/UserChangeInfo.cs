using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm.Api.Organization
{
    /// <summary>
    /// 修改信息
    /// </summary>
    [Serializable]
    public class UserChangeInfo
    {
        public UserChangeInfo(UserInfo userInfo)
        {
            this.ID = userInfo.ID;
            this.Email = userInfo.Email;
            this.Gender = userInfo.Gender;
            this.Name = userInfo.Name;
            this.Remark = userInfo.Remark;
        }

        /// <summary>
        /// ID
        /// </summary>
        public virtual string ID { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public virtual UserGender Gender { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
    }
}
