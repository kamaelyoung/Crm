using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Crm.Api.Organization.Exceptions;
using Crm.Api.Organization;

namespace Crm.Core.Organization
{
    public class DepartmentService : IDepartmentService
    {
        public DepartmentService(OrganizationManagement organizationManager)
        {
            this.OrganizationManager = organizationManager;
        }

        public OrganizationManagement OrganizationManager { set; get; }

        public DepartmentInfo Create(string operationUserId, DepartmentCreateInfo createInfo)
        {
            User opUser = this.OrganizationManager.UserManager.GetUserById(operationUserId);
            if (opUser == null)
            {
                throw new ArgumentException(string.Format("找不到ID为{0}的用户", operationUserId));
            }
            Department department = this.OrganizationManager.DepartmentManager.Create(opUser, createInfo);
            return department.MapDepartmentInfo();
        }

        public void DeleteDepartmentById(string operationUserId, string departmentId)
        {
            User opUser = this.OrganizationManager.UserManager.GetUserById(operationUserId);
            if (opUser == null)
            {
                throw new ArgumentException(string.Format("找不到ID为{0}的用户", operationUserId));
            }
            this.OrganizationManager.DepartmentManager.Delete(opUser, departmentId);
        }

        public void ChangeDepartmentInfo(string operationUserId, DepartmentChangeInfo changeInfo)
        {
            User opUser = this.OrganizationManager.UserManager.GetUserById(operationUserId);
            if (opUser == null)
            {
                throw new ArgumentException(string.Format("找不到ID为{0}的用户", operationUserId));
            }
            if (changeInfo == null)
            {
                throw new ArgumentNullException("departmentInfo");
            }
            Department department = this.OrganizationManager.DepartmentManager.GetDepartmentById(changeInfo.ID);
            if (department == null)
            {
                throw new ArgumentException(string.Format("找不到ID为{0}的部门", changeInfo.ID));
            }
            department.Change(opUser, changeInfo);
        }

        public DepartmentInfo GetDepartmentById(string departmentId)
        {
            Department department = this.OrganizationManager.DepartmentManager.GetDepartmentById(departmentId);
            if (department != null)
            {
                return department.MapDepartmentInfo();
            }
            return null;
        }

        public DepartmentInfo GetParentDepartmentById(string departmentId)
        {
            Department department = this.OrganizationManager.DepartmentManager.GetDepartmentById(departmentId);
            if (department != null)
            {
                department.Parent.MapDepartmentInfo();
            }
            return null;
        }

        public DepartmentInfo GetTopDepartment()
        {
            return this.OrganizationManager.DepartmentManager.TopDepartment.MapDepartmentInfo();
        }

        public IList<DepartmentInfo> GetChildDepartments(string departmentId)
        {
            Department department = this.OrganizationManager.DepartmentManager.GetDepartmentById(departmentId);
            if (department != null)
            {
                return department.Children
                    .Select(x => x.MapDepartmentInfo()).ToList();
            }
            return null;
        }

        public IList<UserInfo> GetUsers(string departmentId)
        {
            Department department = this.OrganizationManager.DepartmentManager.GetDepartmentById(departmentId);
            if (department != null)
            {
                return department.Users.Select(x => x.MapUserInfo()).ToList();
            }
            return null;
        }

		/// <summary>
		/// 根据部门名称获取第一个匹配的部门 (lvxing)
		/// </summary>
		/// <param name="deptName">部门名称</param>
		/// <returns>部门信息</returns>
		public DepartmentInfo GetTopDepartmentByDeptName(string deptName)
		{
			var dept = OrganizationManager.DepartmentManager.GetTopDepartmentByDeptName(deptName);
			return dept != null ? dept.MapDepartmentInfo() : null;
		}

		/// <summary>
		/// 根据部门名称获取部门列表 (lvxing)
		/// </summary>
		/// <param name="deptName">部门名称</param>
		/// <returns>部门信息列表</returns>
		public List<DepartmentInfo> GetDepartmentByDeptName(string deptName)
		{
			var result = new List<DepartmentInfo>();
			var list = OrganizationManager.DepartmentManager.GetDepartmentByDeptName(deptName);
			if (list != null && list.Count > 0)
				result.AddRange(list.Select(item => item.MapDepartmentInfo()).Where(info => info != null));
			return result;
		}

        public List<DepartmentInfo> SearchByName(string name)
        {
            var result = new List<DepartmentInfo>();
            var list = OrganizationManager.DepartmentManager.SearchByName(name);
            if (list != null && list.Count > 0)
                result.AddRange(list.Select(item => item.MapDepartmentInfo()).Where(info => info != null));
            return result;
        }
	}
}