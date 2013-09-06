using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm.Api.Organization
{
    /// <summary>
    /// 功能权限服务
    /// </summary>
    public interface IFunctionService
    {
        FunctionInfo Create(FunctionCreateInfo createInfo);

        FunctionInfo GetFunctionInfoById(string functionId);

        MemberFunctionInfo GetMemberFunction(string memberId, MemberType memberType, string functionId);

        List<MemberFunctionInfo> GetMemberFunction(string functionId);

        List<FunctionInfo> GetAllFunction();

        List<FunctionInfo> GetTopFunctionInfos();

        List<FunctionInfo> GetChildren(string parentId);

        /// <summary>
        /// 获取用户功能
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="parentId">功能权限父id</param>
        /// <returns></returns>
        List<FunctionInfo> GetUserFunctions(string userId, string parentId);

        void SetMemberFunction(string operationUserId, string functionId, string memberId, MemberType memberType, bool hasPermission);

        void DeleteMember(string operationUserId, string memberId, MemberType memberType);

        void DeleteMember(string operationUserId, string functionId, string memberId, MemberType memberType);

        /// <summary>
        /// 检查用户是否有权限
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="functionId">功能权限id</param>
        /// <returns>true表示有权限，false表示无权限</returns>
        bool HasPermission(string userId, string functionId);
    }
}
