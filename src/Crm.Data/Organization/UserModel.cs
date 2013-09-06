using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Crm.Data.Organization
{
    /// <summary>
    /// 用户
    /// </summary>
    public class UserModel
    {
        public UserModel()
        {

        }

        /// <summary>
        /// ID
        /// </summary>
        public virtual string ID { get; set; }

        /// <summary>
        /// 登录帐号
        /// </summary>
        public virtual string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public virtual string Password { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public virtual string Email { get; set; }
        /// 姓名
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public virtual int Gender { get; set; }

        public virtual int Role { set; get; }

        /// <summary>
        /// 用户状态
        /// </summary>
        public virtual int Status { get; set; }

        /// <summary>
        /// 最后一次登录时间
        /// </summary>
        public virtual DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 最后一次登录IP
        /// </summary>
        public virtual string LastLoginIp { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
    }
}
