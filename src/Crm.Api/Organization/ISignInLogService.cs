using System;
using System.Collections.Generic;

using System.Text;

namespace Crm.Api.Organization
{
    public interface ISignInLogService
    {
        IList<SignInLog> GetLogs(SignInLogSearchInfo info, out int count);
    }
}
