using System;
using System.Collections.Generic;

using System.Text;

namespace Crm.Api.Organization
{
    [Serializable]
    public class UserPositionInfo
    {
        public string UserId { set; get; }

        public string UserName { set; get; }

        public string PositionId { set; get; }

        public string PositionName { set; get; }

        public bool Main { set; get; }
    }
}
