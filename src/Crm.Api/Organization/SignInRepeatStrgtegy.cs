using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm.Api.Organization
{
    public enum SignInRepeatStrgtegy
    {
        /// <summary>
        /// 注销已经登录的用户
        /// </summary>
        Logout,

        /// <summary>
        /// 拒绝登录
        /// </summary>
        Deny
    }
}
