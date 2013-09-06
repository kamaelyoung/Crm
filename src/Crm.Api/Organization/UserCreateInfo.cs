using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Crm.Api.Organization
{
    /// <summary>
    /// 创建信息
    /// </summary>
    [Serializable]
    public class UserCreateInfo
    {

        /// <summary>
        /// 用户名
        /// </summary>
        string _account;
        public virtual string Account
        {
            get
            {
                return _account;
            }
            set
            {
                if (value != null)
                {
                    value = value.ToLower().Trim();
                }
                _account = value;
            }
        }

        /// <summary>
        /// 密码
        /// </summary>
        public virtual string Password { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 主职位ID
        /// </summary>
        public virtual string MainPositionId { set; get; }

        /// <summary>
        /// 性别
        /// </summary>
        public virtual UserGender Gender { get; set; }

        /// <summary>
        /// 用户角色
        /// </summary>
        public virtual UserRole Role { set; get; }

        /// <summary>
        /// 用户状态
        /// </summary>
        public virtual UserStatus Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
    }
}
