using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Coldew.Core.Organization
{
    public class PositionUserSearcher
    {
        OrganizationManagement _organizationManager;
        public PositionUserSearcher(OrganizationManagement organizationManager)
        {
            this._organizationManager = organizationManager;
            if (this._organizationManager == null)
            {
                throw new ArgumentNullException("organizationManager");
            }
        }

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        public string PositionId { set; get; }

        /// <summary>
        /// 是否包含子部门用户
        /// </summary>
        public bool Recursive { get; set; }

        public List<User> Search()
        {
            Position position = this._organizationManager.PositionManager
                    .GetPositionById(this.PositionId);
            if (position == null)
            {
                return new List<User>();
            }
            List<User> users = this._organizationManager.UserManager.Search(this.Account, this.Name);
            return users.Where(x => position.InPosition(x, this.Recursive)).ToList();
        }
    }
}
