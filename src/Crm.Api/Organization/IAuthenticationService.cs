using System.Collections.Generic;
using System.Security.Principal;

namespace Crm.Api.Organization
{
    /// <summary>
    /// 提供账号验证，登录相关服务
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// 检查令牌是否有效
        /// </summary>
        /// <param name="token">令牌</param>
        /// <returns></returns>
        void AuthenticateToken(string token);

        /// <summary>
        /// 尝试检查令牌是否有效
        /// </summary>
        /// <param name="token">令牌</param>
        /// <returns>True表示有效，False表示无效</returns>
        bool TryAuthenticateToken(string token);

        /// <summary>
        /// 获取已认证用户
        /// </summary>
        /// <param name="token">令牌</param>
        /// <returns></returns>
        UserInfo GetAuthenticatedUser(string token);

        /// <summary>
        /// 尝试登入
        /// </summary>
        /// <param name="authenticationInfo">验证信息</param>
        /// <param name="clientInfo">客户端信息</param>
        /// <param name="token">令牌，输出参数</param>
        /// <returns></returns>
        SignInResult TrySignIn(string account, string password, string ip, out string token);

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="authenticationInfo">验证信息</param>
        /// <param name="clientInfo">客户端信息</param>
        /// <returns>令牌</returns>
        string SignIn(string account, string password, string ip);

        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="token">令牌</param>
        /// <returns></returns>
        void SignOut(string token);

        /// <summary>
        /// 通过Token获取验证信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns>验证信息</returns>
        AuthenticatedUserInfo GetAuthenticatedUserInfo(string token);
    }
}