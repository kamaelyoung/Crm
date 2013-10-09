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
    /// 提供已认证用户的管理
    /// </summary>
    public class AuthenticatedUserManagement
    {
        Authenticator _authenticator;
        protected Dictionary<string, AuthenticatedUser> _usersByTokenDictionary = new Dictionary<string, AuthenticatedUser>();

        OrganizationManagement _orgMnger;

        public AuthenticatedUserManagement(OrganizationManagement orgMnger)
        {
            this._orgMnger = orgMnger;
            this._authenticator = new Authenticator(orgMnger);
        }

        protected object _addLock = new object();

        public virtual string Add(string account, string password, string ip)
        {
            SignInResult signInResult = SignInResult.OK;
            string token = string.Empty;

            try
            {
                UserSignInChangeInfo signInInfo = null;

                User user = null;
                UserInfo userInfo = null;
                try
                {
                    this._authenticator.Authenticate(account, password, ip);

                    token = this.GetToken(account, ip);
                    if (token != null)
                    {
                        return token;
                    }
                }
                catch (PasswordWrongException)
                {
                    user = this._orgMnger.UserManager.GetUserByAccount(account);
                    userInfo = user.MapUserInfo();
                    signInInfo = new UserSignInChangeInfo(userInfo);
                    signInInfo.LastLoginIp = user.LastLoginIp;
                    signInInfo.LastLoginTime = user.LastLoginTime;
                    user.ChangeSignInInfo(user, signInInfo);
                    throw new PasswordWrongException();
                }
                user = this._orgMnger.UserManager.GetUserByAccount(account);
                userInfo = user.MapUserInfo();

                signInInfo = new UserSignInChangeInfo(userInfo);
                signInInfo.LastLoginTime = DateTime.Now;
                signInInfo.LastLoginIp = ip;
                user.ChangeSignInInfo(user, signInInfo);

                token=this.GenerateToken(account, password, ip);
            }
            catch (OrganizationException ex)
            {
                signInResult = AuthenticationHelper.Map(ex);
                throw ex;
            }
            finally
            {
                this.WriteLog(account, password, ip, signInResult);
            }

            return token;
        }

        protected virtual void WriteLog(string account, string password, string ip, SignInResult signInResult)
        {
            var model = new SignInLogModel()
            {
                Account = account,
                SignInResult = (int)signInResult,
                SignInTime = DateTime.Now,
                Ip = ip,
            };

            NHibernateHelper.CurrentSession.Save(model);
            NHibernateHelper.CurrentSession.Flush();
        }

        protected virtual string GenerateToken(string account, string password, string ip)
        {
            lock (_addLock)
            {
                string newToken = Guid.NewGuid().ToString();
                AuthenticatedUser newAuthedUser = new AuthenticatedUser(account, ip, newToken, DateTime.Now);

                Dictionary<string, AuthenticatedUser> usersByTokenDictionary = this._usersByTokenDictionary.Values.ToDictionary(x => x.Token);
                usersByTokenDictionary.Add(newToken, newAuthedUser);
                this._usersByTokenDictionary = usersByTokenDictionary;
                return newToken;
            }
        }

        protected object _removeLock = new object();

        /// <summary>
        /// 移除已认证用户
        /// </summary>
        /// <param name="token">令牌</param>
        public virtual void Remove(string token)
        {
            if (this._usersByTokenDictionary.ContainsKey(token))
            {
                lock (_removeLock)
                {
                    Dictionary<string, AuthenticatedUser> usersByTokenDictionary = this._usersByTokenDictionary.Values.ToDictionary(x => x.Token);
                    usersByTokenDictionary.Remove(token);
                    this._usersByTokenDictionary = usersByTokenDictionary;
                }
            }
        }

        /// <summary>
        /// 获取令牌对应的已认证用户的账号
        /// </summary>
        /// <param name="token">令牌</param>
        /// <returns>已认证用户的账号。如果令牌无效，则返回null。</returns>
        public AuthenticatedUser GetAuthenticatedUser(string token)
        {
            try
            {
                return this._usersByTokenDictionary[token];
            }
            catch (KeyNotFoundException)
            {

            }
            return null;
        }

        public bool HasToken(string token)
        {
            return this.GetAuthenticatedUser(token) != null;
        }

        public string GetToken(string account, string ipAddress)
        {
            AuthenticatedUser authedUser = _usersByTokenDictionary
                .Where(x => x.Value.Account == account && x.Value.IpAddress.Equals(ipAddress, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Value)
                .FirstOrDefault();
            if (authedUser != null)
            {
                return authedUser.Token;
            }
            return null;
        }

        public List<string> GetUserTokens()
        {
            return this._usersByTokenDictionary.Keys.ToList();
        }
    }
}
