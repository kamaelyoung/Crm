using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Api
{
    public interface IColdewConfigService
    {
        EmailConfigInfo GetEmailConfig();

        void SetEmailConfig(string account, string address, string password, string server);

        void TestEmailConfig(string account, string address, string password, string server);
    }
}
