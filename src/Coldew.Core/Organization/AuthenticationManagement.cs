using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api.Organization.Exceptions;
using Coldew.Data.Organization;
using Coldew.Api.Organization;


namespace Coldew.Core.Organization
{
    /// <summary>
    /// 提供账号验证，登录相关服务
    /// </summary>
    public class AuthenticationManagement
    {
        private readonly UserManagement _userManager;

        protected OrganizationManagement OrgManager { get { return this._orgMnger; } }

        private AuthenticatedUserManagement _authedUserManger;

        private OrganizationManagement _orgMnger;

        /// <summary>
        /// 提供账号验证，登录相关服务
        /// </summary>
        public AuthenticationManagement(OrganizationManagement organizationManager)
        {
            this._userManager = organizationManager.UserManager;

            this._orgMnger = organizationManager;

            this._authedUserManger = new AuthenticatedUserManagement(organizationManager);
        }

        /// <summary>
        /// 检查令牌是否有效
        /// </summary>
        /// <param name="token">令牌</param>
        /// <returns></returns>
        public void AuthenticateToken(string token)
        {
            if (this._authedUserManger.HasToken(token))
            {
                return;
            }
            throw new InvalidTokenException();
        }

        public bool TryAuthenticateToken(string token)
        {
            try
            {
                this.AuthenticateToken(token);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取已认证用户
        /// </summary>
        /// <param name="token">令牌</param>
        /// <returns></returns>
        public AuthenticatedUser GetAuthenticatedUser(string token)
        {
            if (this._authedUserManger.HasToken(token))
            {
                return this._authedUserManger.GetAuthenticatedUser(token);
            }
            return null;
        }

        public SignInResult TrySignIn(string account, string password, string ip, out string token)
        {
            SignInResult result = SignInResult.OK;
            token = null;
            try
            {
                token = this.SignIn(account, password, ip);
            }
            catch (LicenseException)
            {
                throw;
            }
            catch (OrganizationException ex)
            {
                result = AuthenticationHelper.Map(ex);
            }
            return result;
        }


        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="authenticationInfo">验证信息</param>
        /// <param name="clientInfo">客户端信息</param>
        /// <returns>令牌</returns>
        public string SignIn(string account, string password, string ip)
        {
            this._orgMnger.ValidateLicense();
            return this._authedUserManger.Add(account, password, ip);
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="token">令牌</param>
        /// <returns></returns>
        public void SignOut(string token)
        {
            if (this._authedUserManger.HasToken(token))
            {
                this._authedUserManger.Remove(token);
            }
        }
    }
}
