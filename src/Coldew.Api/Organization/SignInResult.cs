using System;
using System.Collections.Generic;
using System.Text;

namespace Coldew.Api.Organization
{
    public enum SignInResult
    {

        OK = 1,

        AccountNotFound = 2,

        AccountLogoffed = 3,

        WrongPassword = 4,

        NeedModifyDefaultPassword = 5,

        AccountLocked = 6,

        AccountExpired = 7,

        PasswordExpired = 8,

        IPDeny = 9,

        Signed = 10,
    }
}
