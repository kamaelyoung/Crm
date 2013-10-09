using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api.Organization;

namespace Crm.Api
{
    [Serializable]
    public class CustomerAreaInfo
    {
        public int ID { set; get; }

        public string Name { set; get; }

        public List<UserInfo> Managers { set; get; }
    }
}
