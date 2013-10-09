using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using Crm.Api;
using Crm.Api.Organization;

namespace Crm.Website.Models
{
    public class ActivityEditModel : JObject
    {
        public ActivityEditModel(ActivityInfo info)
        {
            this.Add("id", info.ID);
            this.Add("subject", info.Subject);
            this.Add("creator", info.Creator.Name);
            this.Add("createTime", info.CreateTime.ToString("yyyy-MM-dd"));
            this.Add("modifiedUser", info.ModifiedUser.Name);
            this.Add("modifiedTime", info.ModifiedTime.ToString("yyyy-MM-dd"));
            foreach (PropertyInfo propertyInfo in info.Metadata.Propertys)
            {
                this.Add(propertyInfo.Code, propertyInfo.ShowValue);
            }
        }
    }
}