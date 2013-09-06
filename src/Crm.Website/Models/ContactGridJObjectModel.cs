using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using Crm.Api;
using System.Web.Mvc;

namespace Crm.Website.Models
{
    public class ContactGridJObjectModel : JObject
    {
        public ContactGridJObjectModel(ContactInfo contactInfo, Controller controller)
        {
            this.Add("id", contactInfo.ID);
            string editUrl = controller.Url.Content(string.Format("~/Contact/Edit?contactId={0}", contactInfo.ID));
            string editLink = string.Format("<a href='{0}' target='_blank'>{1}</a>", editUrl, contactInfo.Name);
            this.Add("name", editLink);
            this.Add("customer", contactInfo.CustomerName);
            this.Add("creator", contactInfo.Creator.Name);
            this.Add("createTime", contactInfo.CreateTime.ToString("yyyy-MM-dd"));
            this.Add("modifiedUser", contactInfo.ModifiedUser.Name);
            this.Add("modifiedTime", contactInfo.ModifiedTime.ToString("yyyy-MM-dd"));
            foreach (PropertyInfo propertyInfo in contactInfo.Metadata.Propertys)
            {
                this.Add(propertyInfo.Code, propertyInfo.Value);
            }
        }

        public ContactGridJObjectModel(ContactInfo contactInfo)
        {
            this.Add("id", contactInfo.ID);
            this.Add("name", contactInfo.Name);
            this.Add("customer", contactInfo.CustomerName);
            this.Add("creator", contactInfo.Creator.Name);
            this.Add("createTime", contactInfo.CreateTime.ToString("yyyy-MM-dd"));
            this.Add("modifiedUser", contactInfo.ModifiedUser.Name);
            this.Add("modifiedTime", contactInfo.ModifiedTime.ToString("yyyy-MM-dd"));
            foreach (PropertyInfo propertyInfo in contactInfo.Metadata.Propertys)
            {
                this.Add(propertyInfo.Code, propertyInfo.Value);
            }
        }
    }
}