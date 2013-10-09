using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Collections;
using Coldew.Api;

namespace Crm.Website.Models
{
    public class MetadataEditModel : JObject
    {
        public MetadataEditModel(MetadataInfo customerInfo)
        {
            this.Add("id", customerInfo.ID);
            foreach (PropertyInfo propertyInfo in customerInfo.Propertys)
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