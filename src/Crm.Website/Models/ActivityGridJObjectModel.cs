using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using Crm.Api;
using System.Web.Mvc;

namespace Crm.Website.Models
{
    public class ActivityGridJObjectModel : JObject
    {
        public ActivityGridJObjectModel(ActivityInfo activity, Controller controller)
        {
            this.Add("id", activity.ID);
            string editUrl = controller.Url.Content(string.Format("~/Activity/Edit?activityId={0}", activity.ID));
            string editLink = string.Format("<a href='{0}' target='_blank'>{1}</a>", editUrl, activity.Subject);
            this.Add("subject", editLink);
            this.Add("contact", activity.ContactName);
            this.Add("customer", activity.CustomerName);
            this.Add("creator", activity.Creator.Name);
            this.Add("createTime", activity.CreateTime.ToString("yyyy-MM-dd"));
            this.Add("modifiedUser", activity.ModifiedUser.Name);
            this.Add("modifiedTime", activity.ModifiedTime.ToString("yyyy-MM-dd"));
            foreach (PropertyInfo propertyInfo in activity.Metadata.Propertys)
            {
                this.Add(propertyInfo.Code, propertyInfo.ShowValue);
            }
        }
    }
}