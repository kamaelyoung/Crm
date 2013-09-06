using System.Collections.Generic;

namespace Crm.Api.Organization
{
    public interface IPositionService
    {
        /// <summary>
        /// 创建职位
        /// </summary>
        /// <param name="operationUserId"></param>
        /// <param name="createInfo"></param>
        /// <returns></returns>
        PositionInfo Create(string operationUserId, PositionCreateInfo createInfo);

        /// <summary>
        /// 删除职位
        /// </summary>
        /// <param name="positionId">职位ID</param>
        void DeletePositionById(string operationUserId, string positionId);

        /// <summary>
        /// 修改职位
        /// </summary>
        /// <param name="operationUserId"></param>
        /// <param name="changeInfo"></param>
        void ChangePositionInfo(string operationUserId, PositionChangeInfo changeInfo);

        /// <summary>
        /// 向职位下添加用户
        /// </summary>
        /// <param name="positionId">职位ID</param>
        /// <param name="userId">用户ID</param>
        void AddUserToPosition(string operationUserId, string positionId, string userId);

        void AddUserToPosition(string operationUserId, UserPositionInfo userPositionInfo);

        /// <summary>
        /// 从职位中移除用户
        /// </summary>
        /// <param name="positionId">职位ID</param>
        /// <param name="userId">用户ID</param>
        void RemoveUserFromPosition(string operationUserId, string positionId, string userId);

        /// <summary>
        /// 获取职位
        /// </summary>
        /// <param name="positionId">职位ID</param>
        /// <returns>职位</returns>
        PositionInfo GetPositionById(string positionId);

        PositionInfo GetPositionByName(string name);

        PositionInfo GetTopPosition();

        /// <summary>
        /// 获取子职位
        /// </summary>
        /// <param name="positionId">职位ID</param>
        /// <returns>子职位集合</returns>
        IList<PositionInfo> GetChildPositions(string positionId);

        /// <summary>
        /// 管理部门
        /// </summary>
        /// <param name="token"></param>
        /// <param name="positionId">职位ID</param>
        /// <returns>子职位集合</returns>
        IList<DepartmentInfo> GetManagerialDepartments(string positionId);

        /// <summary>
        /// 获取部门所属职位列表
        /// </summary>
        /// <param name="departmentId">部门ID</param>
        /// <returns>职位列表</returns>
        IList<PositionInfo> GetPositionsByDepartmentId(string departmentId);

        /// <summary>
        /// 获取用户所属职位
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        IList<PositionInfo> GetPositionsOfUser(string userId);

        IList<UserPositionInfo> GetUserPositionsOfUser(string userId);

        UserPositionInfo GetUserPositions(string userId, string positionId);

        void ChangeUserPosition(string operationUserId, string userId, string positionId, string changePositionId);

        /// <summary>
        /// 搜索用户名称
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IList<PositionInfo> SearchByName(string name);
    }
}