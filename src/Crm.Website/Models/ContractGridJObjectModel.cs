using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using Crm.Api;
using System.Web.Mvc;

namespace Crm.Website.Models
{
    public class ContractGridJObjectModel : JObject
    {
        public ContractGridJObjectModel(ContractInfo contract, Controller controller)
        {
            this.Add("id", contract.ID);
            string editUrl = controller.Url.Content(string.Format("~/Contract/Edit?contractId={0}", contract.ID));
            string editLink = string.Format("<a href='{0}' target='_blank'>{1}</a>", editUrl, contract.Name);
            this.Add("name", editLink);
            this.Add("customer", contract.CustomerName);
            this.Add("endDate", contract.EndDate.ToString("yyyy-MM-dd"));
            this.Add("value", contract.Value);
            this.Add("expiredComputeDays", contract.ExpiredComputeDays);
            this.Add("owners", string.Join(",", contract.Owners.Select(x => x.Name)));
            this.Add("startDate", contract.StartDate.ToString("yyyy-MM-dd"));
            this.Add("creator", contract.Creator.Name);
            this.Add("createTime", contract.CreateTime.ToString("yyyy-MM-dd"));
            this.Add("modifiedUser", contract.ModifiedUser.Name);
            this.Add("modifiedTime", contract.ModifiedTime.ToString("yyyy-MM-dd"));
            foreach (PropertyInfo propertyInfo in contract.Metadata.Propertys)
            {
                this.Add(propertyInfo.Code, propertyInfo.ShowValue);
            }
        }
        public ContractGridJObjectModel(ContractInfo contract)
        {
            this.Add("id", contract.ID);
            this.Add("name", contract.Name);
            this.Add("customer", contract.CustomerName);
            this.Add("endDate", contract.EndDate.ToString("yyyy-MM-dd"));
            this.Add("value", contract.Value);
            this.Add("expiredComputeDays", contract.ExpiredComputeDays);
            this.Add("owners", string.Join(",", contract.Owners.Select(x => x.Name)));
            this.Add("startDate", contract.StartDate.ToString("yyyy-MM-dd"));
            this.Add("creator", contract.Creator.Name);
            this.Add("createTime", contract.CreateTime.ToString("yyyy-MM-dd"));
            this.Add("modifiedUser", contract.ModifiedUser.Name);
            this.Add("modifiedTime", contract.ModifiedTime.ToString("yyyy-MM-dd"));
            foreach (PropertyInfo propertyInfo in contract.Metadata.Propertys)
            {
                this.Add(propertyInfo.Code, propertyInfo.ShowValue);
            }
        }
    }
}