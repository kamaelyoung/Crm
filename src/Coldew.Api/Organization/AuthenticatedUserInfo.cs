using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Api.Organization
{
    [Serializable]
    public class AuthenticatedUserInfo
    {
        public string Account { set; get; }

        public string IpAddress { set; get; }

        public string Token { set; get; }

        public DateTime LoginTime { set; get; }
    }
}
