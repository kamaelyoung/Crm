using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using Crm.Api;
using Crm.Api.Organization;
using System.Collections;

namespace Crm.Website.Models
{
    public class CustomerEditModel : JObject
    {
        public CustomerEditModel(CustomerInfo customerInfo)
        {
            this.Add("id", customerInfo.ID);
            this.Add("name", customerInfo.Name);
            this.Add("area", customerInfo.Area.ID);
            JArray array = new JArray();
            foreach (UserInfo userInfo in customerInfo.SalesUsers)
            {
                array.Add(userInfo.Account);
            }
            this.Add("salesAccounts", array);
            this.Add("creator", customerInfo.Creator.Name);
            this.Add("createTime", customerInfo.CreateTime.ToString("yyyy-MM-dd"));
            this.Add("modifiedUser", customerInfo.ModifiedUser.Name);
            this.Add("modifiedTime", customerInfo.ModifiedTime.ToString("yyyy-MM-dd"));
            foreach (PropertyInfo propertyInfo in customerInfo.Metadata.Propertys)
            {
                JToken token = null;
                if (propertyInfo.EditValue is IEnumerable)
                {
                    token = new JArray(propertyInfo.EditValue);
                }
                else
                {
                    token = propertyInfo.EditValue;
                }
                this.Add(propertyInfo.Code, token);
            }
        }
    }
}