using System.Collections.Generic;
using System.Data;
using System.Collections.ObjectModel;


namespace Coldew.Api.Organization
{
    public interface IUserService
    {
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="operationUserId">操作人id</param>
        /// <param name="createInfo">创建信息</param>
        /// <returns>用户信息</returns>
        UserInfo Create(string operationUserId, UserCreateInfo createInfo);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="operationUserId">操作人id</param>
        /// <param name="userId">要删除的用户id</param>
        void Delete(string operationUserId, string userId);

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="operationUserId">操作人id</param>
        /// <param name="changeInfo">修改信息</param>
        void ChangeInfo(string operationUserId, UserChangeInfo changeInfo);

        /// <summary>
        /// 修改登录信息
        /// </summary>
        /// <param name="operationUserId">操作人id</param>
        /// <param name="changeInfo">修改信息</param>
        void ChangeSignInInfo(string operationUserId, UserSignInChangeInfo changeInfo);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userId">操作人id</param>
        /// <param name="oldPassword">旧密码</param>
        /// <param name="newPassword">新密码</param>
        void ChangePassword(string account, string oldPassword, string newPassword);

        /// <summary>
        /// 通过密码策略重置密码
        /// </summary>
        /// <param name="userId">用户id</param>
        void ResetPassword(string operationUserId, string userId, string newPassword);

        /// <summary>
        /// 导入密码
        /// </summary>
        /// <param name="operationUserId">操作人id</param>
        /// <param name="userId">用户id</param>
        /// <param name="password">md5加密以后的密码</param>
        void ImportPassword(string operationUserId, string userId, string password);
        /// <summary>
        /// 根据用户ID获取用户
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns>用户信息</returns>
        UserInfo GetUserById(string userId);

        /// <summary>
        /// 获取所有的用户
        /// </summary>
        /// <returns>用户集合</returns>
        IList<UserInfo> GetAllUser();

        IList<UserInfo> GetAllNormalUser();

        /// <summary>
        /// 获取所有被注销的用户
        /// </summary>
        /// <returns>用户集合</returns>
        IList<UserInfo> GetLogoffedUsers();

        /// <summary>
        /// 根据账号获取用户
        /// </summary>
        /// <param name="userAccount">用户账号</param>
        /// <returns>用户信息</returns>
        UserInfo GetUserByAccount(string userAccount);

        /// <summary>
        /// 获取部门中的用户列表
        /// </summary>
        /// <param name="departmentId">部门ID</param>
        /// <returns>用户集合</returns>
        IList<UserInfo> GetUsersInDepartment(string departmentId);

        /// <summary>
        /// 获取职位下的用户列表
        /// </summary>
        /// <param name="positionId">职位ID</param>
        /// <returns>用户集合</returns>
        IList<UserInfo> GetUsersInPosition(string positionId);

        /// <summary>
        /// 获取用户的所属部门ID列表
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns>部门id集合</returns>
        IList<string> GetDepartmentIdList(string userId);

        /// <summary>
        /// 获取用户的所属部门列表
        /// </summary>
        /// <param name="userId">用户集合</param>
        /// <returns>部门信息集合</returns>
        IList<DepartmentInfo> GetDepartmentList(string userId);

        /// <summary>
        /// 获取用户的所属职位ID列表
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns>职位id集合</returns>
        IList<string> GetPositionIdList(string userId);

        /// <summary>
        /// 获取用户的所属职位列表
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns>职位信息集合</returns>
        IList<PositionInfo> GetPositionList(string userId);

        /// <summary>
        /// 获取用户的所属用户组ID列表
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns>用户组id集合</returns>
        IList<string> GetGroupIdList(string userId);

        /// <summary>
        /// 获取用户的所属用户组列表
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns>用户组集合</returns>
        IList<GroupInfo> GetGroupList(string userId);

        /// <summary>
        /// 获取用户组下的用户列表
        /// </summary>
        /// <param name="groupId">用户组ID</param>
        /// <returns>用户集合</returns>
        IList<UserInfo> GetUsersInGroup(string groupId);

        /// <summary>
        /// 锁定用户
        /// </summary>
        /// <param name="operationUserId">操作人id</param>
        /// <param name="userId">用户id</param>
        void Lock(string operationUserId, string userId);

        /// <summary>
        /// 解锁用户
        /// </summary>
        /// <param name="operationUserId">操作人id</param>
        /// <param name="userId">用户id</param>
        void Unlock(string operationUserId, string userId);

        /// <summary>
        /// 注销用户
        /// </summary>
        /// <param name="operationUserId">操作人id</param>
        /// <param name="userId">用户id</param>
        void Logoff(string operationUserId, string userId);

        /// <summary>
        /// 激活用户
        /// </summary>
        /// <param name="operationUserId">操作人id</param>
        /// <param name="userId">用户id</param>
        void Activate(string operationUserId, string userId);

        /// <summary>
        /// 搜索用户
        /// </summary>
        /// <param name="filterInfo">搜索信息</param>
        /// <returns>用户集合</returns>
        IList<UserInfo> SearchUser(UserFilterInfo filterInfo);

        /// <summary>
        /// 搜索用户
        /// </summary>
        /// <param name="accountOrName">用户名或姓名</param>
        /// <returns>用户集合</returns>
        IList<UserInfo> SearchUser(string accountOrName);

        /// <summary>
        /// 获取system用户信息
        /// </summary>
        /// <returns>system用户信息</returns>
        UserInfo GetSystem();

        /// <summary>
        /// 获取用户数
        /// </summary>
        /// <returns></returns>
        int GetUsersCount();

        List<UserInfo> GetAllUnderling(string userId);
    }
}