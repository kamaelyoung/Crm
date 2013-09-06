using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Api.Organization;

namespace Crm.Api
{
    [Serializable]
    public class CustomerInfo
    {
        public string ID { set; get; }

        public string Name { set; get; }

        public CustomerAreaInfo Area { set; get; }

        public List<UserInfo> SalesUsers { set; get; }

        public UserInfo Creator { set; get; }

        public DateTime CreateTime { set; get; }

        public UserInfo ModifiedUser { set; get; }

        public DateTime ModifiedTime { set; get; }

        public MetadataInfo Metadata { set; get; }
    }
}
