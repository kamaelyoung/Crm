using System.Collections.Generic;
using System.Collections.ObjectModel;

using System;

namespace Crm.Api.Organization
{
    /// <summary>
    /// 用户过滤器
    /// </summary>
    [Serializable]
    public class UserFilterInfo
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 帐号或姓名
        /// </summary>
        public string AccountOrName { set; get; }

        public string OrganizationId { set; get; }

        public OrganizationType OrganizationType { set; get; }

        /// <summary>
        /// 是否包含子部门用户
        /// </summary>
        public bool Recursive { get; set; }
    }
}
