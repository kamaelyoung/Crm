using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Coldew.Data.Organization
{
    /// <summary>
    /// 登录日志
    /// </summary>
    public class SignInLogModel
    {
        public SignInLogModel()
        {
        }

        /// <summary>
        /// ID
        /// </summary>
        public virtual long ID { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public virtual string Account { get; set; }


        /// <summary>
        /// 登录结果
        /// </summary>
        public virtual int SignInResult { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public virtual DateTime SignInTime { get; set; }


        /// <summary>
        /// 客户端信息
        /// </summary>
        public virtual string Ip { get; set; }
    }
}
