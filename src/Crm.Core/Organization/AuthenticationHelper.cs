using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Api.Organization.Exceptions;
using Crm.Api.Organization;

namespace Crm.Core.Organization
{
    public class AuthenticationHelper
    {
        public static SignInResult Map(OrganizationException ex)
        {
            SignInResult result;
            if (ex is AccountLogoffException)
            {
                result = SignInResult.AccountLogoffed;
            }
            else if (ex is AccountLockedException)
            {
                result = SignInResult.AccountLocked;
            }
            else if (ex is AccountNotFoundException)
            {
                result = SignInResult.AccountNotFound;
            }
            else if (ex is NeedModifyDefaultPasswordException)
            {
                result = SignInResult.NeedModifyDefaultPassword;
            }
            else if (ex is PasswordExpiredException)
            {
                result = SignInResult.PasswordExpired;
            }
            else if (ex is PasswordWrongException)
            {
                result = SignInResult.WrongPassword;
            }
            else if (ex is AccountExpiredException)
            {
                result = SignInResult.AccountExpired;
            }
            else if (ex is IPDenyExpcetion)
            {
                result = SignInResult.IPDeny;
            }
            else if (ex is AccountSignedException)
            {
                result = SignInResult.Signed;
            }
            else
            {
                throw ex;
            }
            return result;
        }
    }
}
