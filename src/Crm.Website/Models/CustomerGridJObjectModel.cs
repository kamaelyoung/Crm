using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using Crm.Api;
using System.Web.Mvc;

namespace Crm.Website.Models
{
    public class CustomerGridJObjectModel : JObject
    {
        public CustomerGridJObjectModel(CustomerInfo customerInfo, Dictionary<string, CustomerInfo> myFavorites, Controller controller)
        {
            this.Add("id", customerInfo.ID);
            string favoritedIcon = "";
            if (myFavorites.ContainsKey(customerInfo.ID))
            {
                favoritedIcon = "<i class='icon-star'></i>";
            }
            string editUrl = controller.Url.Content(string.Format("~/Customer/Edit?customerId={0}", customerInfo.ID));
            string editLink = string.Format("<a href='{0}' target='_blank'>{1}</a>", editUrl, customerInfo.Name);
            this.Add("name", favoritedIcon + editLink);
            this.Add("area", customerInfo.Area.Name);
            this.Add("salesUsers", string.Join(",", customerInfo.SalesUsers.Select(x => x.Name)));
            this.Add("creator", customerInfo.Creator.Name);
            this.Add("createTime", customerInfo.CreateTime.ToString("yyyy-MM-dd"));
            this.Add("modifiedUser", customerInfo.ModifiedUser.Name);
            this.Add("modifiedTime", customerInfo.ModifiedTime.ToString("yyyy-MM-dd"));
            foreach (PropertyInfo propertyInfo in customerInfo.Metadata.Propertys)
            {
                this.Add(propertyInfo.Code, propertyInfo.Value);
            }
        }

        public CustomerGridJObjectModel(CustomerInfo customerInfo, Controller controller)
        {
            this.Add("id", customerInfo.ID);
            string editUrl = controller.Url.Content(string.Format("~/Customer/Edit?customerId={0}", customerInfo.ID));
            string editLink = string.Format("<a href='{0}' target='_blank'>{1}</a>", editUrl, customerInfo.Name);
            this.Add("name", editLink);
            this.Add("area", customerInfo.Area.Name);
            this.Add("salesUsers", string.Join(",", customerInfo.SalesUsers.Select(x => x.Name)));
            this.Add("creator", customerInfo.Creator.Name);
            this.Add("createTime", customerInfo.CreateTime.ToString("yyyy-MM-dd"));
            this.Add("modifiedUser", customerInfo.ModifiedUser.Name);
            this.Add("modifiedTime", customerInfo.ModifiedTime.ToString("yyyy-MM-dd"));
            foreach (PropertyInfo propertyInfo in customerInfo.Metadata.Propertys)
            {
                this.Add(propertyInfo.Code, propertyInfo.Value);
            }
        }

        public CustomerGridJObjectModel(CustomerInfo customerInfo)
        {
            this.Add("id", customerInfo.ID);
            this.Add("name", customerInfo.Name);
            this.Add("area", customerInfo.Area.Name);
            this.Add("salesUsers", string.Join(",", customerInfo.SalesUsers.Select(x => x.Name)));
            this.Add("creator", customerInfo.Creator.Name);
            this.Add("createTime", customerInfo.CreateTime.ToString("yyyy-MM-dd"));
            this.Add("modifiedUser", customerInfo.ModifiedUser.Name);
            this.Add("modifiedTime", customerInfo.ModifiedTime.ToString("yyyy-MM-dd"));
            foreach (PropertyInfo propertyInfo in customerInfo.Metadata.Propertys)
            {
                this.Add(propertyInfo.Code, propertyInfo.Value);
            }
        }
    }
}