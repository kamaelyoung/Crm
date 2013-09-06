using System.Collections.Generic;

namespace Crm.Api.Organization
{
    public interface IGroupService
    {
        /// <summary>
        /// 创建组
        /// </summary>
        /// <param name="operationUserId">操作人id</param>
        /// <param name="createInfo">创建信息</param>
        /// <returns>用户组信息</returns>
        GroupInfo Create(string operationUserId, GroupCreateInfo createInfo);

        /// <summary>
        /// 删除组
        /// </summary>
        /// <param name="operationUserId">操作人id</param>
        /// <param name="groupId">用户组id</param>
        void DeleteGroupById(string operationUserId, string groupId);

        /// <summary>
        /// 修改组
        /// </summary>
        /// <param name="operationUserId">操作人id</param>
        /// <param name="changeInfo">修改信息</param>
        void ChangeGroupInfo(string operationUserId, GroupChangeInfo changeInfo);

        /// <summary>
        /// 从用户组中添加成员
        /// </summary>
        /// <param name="operationUserId">操作人id</param>
        /// <param name="memberInfo">成员信息</param>
        void AddMemberToGroup(string operationUserId, GroupMemberInfo memberInfo);
        
        /// <summary>
        /// 从用户组中移除成员
        /// </summary>
        /// <param name="operationUserId"></param>
        /// <param name="memberInfo"></param>
        void RemoveMemberFromGroup(string operationUserId, GroupMemberInfo memberInfo);

        /// <summary>
        /// 向用户组中添加用户
        /// </summary>
        /// <param name="groupId">用户组ID</param>
        /// <param name="userId">用户ID</param>
        void AddUserToGroup(string operationUserId, string groupId, string userId);

        /// <summary>
        /// 从用户组中移除用户
        /// </summary>
        /// <param name="groupId">用户组ID</param>
        /// <param name="userId">用户ID</param>
        void RemoveUserFromGroup(string operationUserId, string groupId, string userId);

        /// <summary>
        /// 向用户组中添加职位
        /// </summary>
        /// <param name="groupId">用户组ID</param>
        /// <param name="positionId">职位ID</param>
        void AddPositionToGroup(string operationUserId, string groupId, string positionId);

        /// <summary>
        /// 从用户组中移除职位
        /// </summary>
        /// <param name="groupId">用户组ID</param>
        /// <param name="positionId">职位ID</param>
        void RemovePositionFromGroup(string operationUserId, string groupId, string positionId);

        /// <summary>
        /// 向用户组中添加部门
        /// </summary>
        /// <param name="groupId">用户组ID</param>
        /// <param name="departmentId">部门ID</param>
        void AddDepartmentToGroup(string operationUserId, string groupId, string departmentId);

        /// <summary>
        /// 从用户组中移除部门
        /// </summary>
        /// <param name="groupId">用户组ID</param>
        /// <param name="departmentId">部门ID</param>
        void RemoveDepartmentFromGroup(string operationUserId, string groupId, string departmentId);

        /// <summary>
        /// 添加用户组到用户组
        /// </summary>
        /// <param name="operationUserId">操作人id</param>
        /// <param name="toGroupId">添加进的用户组id</param>
        /// <param name="groupId">被添加的用户组id</param>
        void AddGroupToGroup(string operationUserId, string toGroupId, string groupId);

        /// <summary>
        /// 从用户组中移除用户组
        /// </summary>
        /// <param name="operationUserId">操作人id</param>
        /// <param name="fromGroupId">移除的工作组id</param>
        /// <param name="groupId">被移除的用户组id</param>
        void RemoveGroupFromGroup(string operationUserId, string fromGroupId, string groupId);

        /// <summary>
        /// 通过用户组id获取用户组
        /// </summary>
        /// <param name="groupId">用户组ID</param>
        /// <returns>用户组信息</returns>
        GroupInfo GetGroupById(string groupId);

        /// <summary>
        /// 获取所有的用户组
        /// </summary>
        /// <returns></returns>
        IList<GroupInfo> GetGroups();

        /// <summary>
        /// 通过用户id，获取个人用户组
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns>用户组集合</returns>
        IList<GroupInfo> GetPersonalGroups(string userId);

        /// <summary>
        /// 通过个人信息获取用户组
        /// </summary>
        /// <returns></returns>
        IList<GroupInfo> GetAllPersonalGroups();

        /// <summary>
        /// 获取所属用户组成员列表
        /// </summary>
        /// <param name="groupId">用户组ID</param>
        /// <returns>用户组成员列表</returns>
        IList<GroupMemberInfo> GetGroupMembersByGroupId(string groupId);
        
        /// <summary>
        /// 搜索用户名称
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IList<GroupInfo> SearchByName(string name);
    }
}
