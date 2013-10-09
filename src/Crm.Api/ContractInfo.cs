using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api.Organization;
using Coldew.Api;

namespace Crm.Api
{
    [Serializable]
    public class ContractInfo
    {
        public string ID { set; get; }

        public string Name { set; get; }

        public string CustomerId { set; get; }

        public string CustomerName { set; get; }

        public DateTime StartDate { set; get; }

        public DateTime EndDate { set; get; }

        public int ExpiredComputeDays { set; get; }

        public float Value { set; get; }

        public List<UserInfo> Owners { set; get; }

        public UserInfo Creator { set; get; }

        public DateTime CreateTime { set; get; }

        public UserInfo ModifiedUser { set; get; }

        public DateTime ModifiedTime { set; get; }

        public MetadataInfo Metadata { set; get; }
    }
}
