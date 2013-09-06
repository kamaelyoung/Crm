using System.Collections.Generic;

namespace Crm.Api.Organization
{
    public interface IDepartmentService
    {
        /// <summary>
        /// 创建部门
        /// </summary>
        /// <param name="operationUserId">操作人id</param>
        /// <param name="createInfo">创建信息</param>
        /// <returns></returns>
        DepartmentInfo Create(string operationUserId, DepartmentCreateInfo createInfo);

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="operationUserId">操作人id</param>
        /// <param name="departmentId">部门id</param>
        void DeleteDepartmentById(string operationUserId, string departmentId);

        /// <summary>
        /// 修改部门信息
        /// </summary>
        /// <param name="operationUserId">操作人id</param>
        /// <param name="changeInfo">修改信息</param>
        void ChangeDepartmentInfo(string operationUserId, DepartmentChangeInfo changeInfo);

        /// <summary>
        /// 通过部门ID获取部门信息
        /// </summary>
        /// <param name="departmentId">部门ID</param>
        /// <returns>部门信息</returns>
        DepartmentInfo GetDepartmentById(string departmentId);

        /// <summary>
        /// 根据部门ID获取父部门
        /// </summary>
        /// <param name="departmentId">部门ID</param>
        /// <returns>父部门</returns>
        DepartmentInfo GetParentDepartmentById(string departmentId);

        /// <summary>
        /// 获取系统顶层部门
        /// </summary>
        /// <returns></returns>
        DepartmentInfo GetTopDepartment();

        /// <summary>
        /// 根据部门ID获取子部门
        /// </summary>
        /// <param name="departmentId">部门ID</param>
        /// <returns>部门结果集</returns>
        IList<DepartmentInfo> GetChildDepartments(string departmentId);

        /// <summary>
        /// 通过部门id获取部门用户
        /// </summary>
        /// <param name="departmentId">部门id</param>
        /// <returns>部门的用户</returns>
        IList<UserInfo> GetUsers(string departmentId);

		/// <summary>
		/// 根据部门名称获取第一个匹配的部门 (lvxing)
		/// </summary>
		/// <param name="deptName">部门名称</param>
		/// <returns>部门信息</returns>
		DepartmentInfo GetTopDepartmentByDeptName(string deptName);

		/// <summary>
		/// 根据部门名称获取部门列表 (lvxing)
		/// </summary>
		/// <param name="deptName">部门名称</param>
		/// <returns>部门信息列表</returns>
		List<DepartmentInfo> GetDepartmentByDeptName(string deptName);

        /// <summary>
        /// 模糊搜索部门名称
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        List<DepartmentInfo> SearchByName(string name);
    }
}
