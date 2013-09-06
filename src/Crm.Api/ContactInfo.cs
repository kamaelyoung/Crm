using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Api.Organization;

namespace Crm.Api
{
    [Serializable]
    public class ContactInfo
    {
        public string ID { set; get; }

        public string Name { set; get; }

        public string CustomerId { set; get; }

        public string CustomerName { set; get; }

        public UserInfo Creator {  set; get; }

        public DateTime CreateTime {  set; get; }

        public UserInfo ModifiedUser {  set; get; }

        public DateTime ModifiedTime {  set; get; }

        public MetadataInfo Metadata { set; get; }
    }
}
