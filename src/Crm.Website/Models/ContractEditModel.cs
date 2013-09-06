using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using Crm.Api;
using Crm.Api.Organization;

namespace Crm.Website.Models
{
    public class ContractEditModel : JObject
    {
        public ContractEditModel(ContractInfo info)
        {
            this.Add("id", info.ID);
            this.Add("name", info.Name);
            this.Add("endDate", info.EndDate.ToString("yyyy-MM-dd"));
            this.Add("expiredComputeDays", info.ExpiredComputeDays);
            JArray array = new JArray();
            foreach (UserInfo userInfo in info.Owners)
            {
                array.Add(userInfo.Account);
            }
            this.Add("owners", array);
            this.Add("value", info.Value);
            this.Add("startDate", info.StartDate.ToString("yyyy-MM-dd"));
            this.Add("creator", info.Creator.Name);
            this.Add("createTime", info.CreateTime.ToString("yyyy-MM-dd"));
            this.Add("modifiedUser", info.ModifiedUser.Name);
            this.Add("modifiedTime", info.ModifiedTime.ToString("yyyy-MM-dd"));
            foreach (PropertyInfo propertyInfo in info.Metadata.Propertys)
            {
                this.Add(propertyInfo.Code, propertyInfo.Value);
            }
        }
    }
}