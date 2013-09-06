using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Crm.Core.Organization
{
    public class DepartmentUserSearcher
    {
        OrganizationManagement _organizationManager;
        public DepartmentUserSearcher(OrganizationManagement organizationManager)
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

        public string AccountOrName { set; get; }

        public string DepartmentId { set; get; }

        /// <summary>
        /// 是否包含子部门用户
        /// </summary>
        public bool Recursive { get; set; }

        public List<User> Search()
        {
            Department department = this._organizationManager.DepartmentManager
                    .GetDepartmentById(this.DepartmentId);
            if (department == null)
            {
                return new List<User>();
            }

            if (string.IsNullOrEmpty(Account) && string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(AccountOrName))
            {
                return department.AllUsers.ToList();
            }

            if (string.IsNullOrEmpty(AccountOrName))
            {
                return this._organizationManager.UserManager
                       .Search(this.Account, this.Name)
                       .Where(x => department.InDepartment(x, this.Recursive))
                       .ToList();
            }
            else
            {
                return this._organizationManager.UserManager
                    .Search(this.AccountOrName)
                    .Where(x => department.InDepartment(x, this.Recursive))
                    .ToList();
            }
        }
    }
}
