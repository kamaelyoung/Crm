using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api.Organization.Exceptions;
using Coldew.Api.Organization;

namespace Coldew.Core.Organization
{
    public class Authenticator
    {
        OrganizationManagement _orgManager;
        public Authenticator(OrganizationManagement orgManager)
        {
            _orgManager = orgManager;
        }

        public void Authenticate(string account, string password, string ip)
        {
            var user = this._orgManager.UserManager.GetUserByAccount(account);
            if (user == null)
            {
                throw new AccountNotFoundException();
            }
            else if (user.Status == UserStatus.Lock)
            {
                throw new AccountLockedException();
            }
            else if (user.Status == UserStatus.Logoff)
            {
                throw new AccountLogoffException();
            }

            this.AuthenticatePassword(user, account, password);
        }

        protected virtual void AuthenticatePassword(User user, string account, string password)
        {
            // 比对口令
            bool isPasswordMatch = this._orgManager.UserManager.CheckPassword(account, password);
            if (!isPasswordMatch)
            {
                throw new PasswordWrongException();
            }
        }

    }
}
