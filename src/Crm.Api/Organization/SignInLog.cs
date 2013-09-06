using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Crm.Api.Organization
{
    /// <summary>
    /// 登录日志
    /// </summary>
    [Serializable]
    public class SignInLog
    {
        /// <summary>
        /// ID
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }


        /// <summary>
        /// 登录结果
        /// </summary>
        public SignInResult Result { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime SignInTime { get; set; }

        /// <summary>
        /// App Key
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 客户端Ip
        /// </summary>
        public string Ip { get; set; }
    }
}
