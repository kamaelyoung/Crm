using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Api.Organization
{
    /// <summary>
    /// 功能权限服务
    /// </summary>
    public interface IFunctionService
    {
        FunctionInfo GetFunctionInfoById(string functionId);

        List<FunctionInfo> GetAllFunction();

        /// <summary>
        /// 检查用户是否有权限
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="functionId">功能权限id</param>
        /// <returns>true表示有权限，false表示无权限</returns>
        bool HasPermission(string userId, string functionId);
    }
}
