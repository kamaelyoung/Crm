using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Crm.Api;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace Crm.Website.Models
{
    public class CustomerSearchModel
    {
        public string keyword;
        public List<JObject> extendSearchs;


        public CustomerSearchInfo Map()
        {
            List<string> keywords = null;
            if (!string.IsNullOrEmpty(this.keyword))
            {
                Regex regex = new Regex("\\s+");
                this.keyword = regex.Replace(this.keyword, " ");
                keywords = keyword.Split(' ').ToList();
            }
            List<PropertySearchInfo> propertySearchInfos = null;
            if (this.extendSearchs != null)
            {
                propertySearchInfos = ExtendHelper.MapPropertySearchInfos(extendSearchs);
            }
            if (propertySearchInfos != null || keywords != null)
            {
                return new CustomerSearchInfo
                {
                    PropertySearchInfos = propertySearchInfos,
                    Keywords = keywords
                };
            }

            return null;
        }
    }
}