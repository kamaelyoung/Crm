using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Crm.Api;

namespace Crm.Website.Models
{
    public class CustomerModel
    {
        public CustomerModel(CustomerInfo customerInfo)
        {
            this.id = customerInfo.ID;
            this.name = customerInfo.Name;
            this.area = customerInfo.Area.Name;
            this.salesUsers = string.Join(",", customerInfo.SalesUsers);
            this.creator = customerInfo.Creator.Name;
            this.createTime = customerInfo.CreateTime.ToString("yyyy-MM-dd");
            this.modifiedUser = customerInfo.ModifiedUser.Name;
            this.modifiedTime = customerInfo.ModifiedTime.ToString("yyyy-MM-dd");
        }

        public string id { set; get; }

        public string name { set; get; }

        public string area { set; get; }

        public string salesUsers { set; get; }

        public string creator { set; get; }

        public string createTime { set; get; }

        public string modifiedUser { set; get; }

        public string modifiedTime { set; get; }
    }
}