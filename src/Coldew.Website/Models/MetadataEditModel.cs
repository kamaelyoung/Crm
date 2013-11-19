using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Collections;
using Coldew.Api;
using Newtonsoft.Json;

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
                if (propertyInfo.FieldType == FieldType.UserList || propertyInfo.FieldType == FieldType.CheckboxList)
                {
                    token = new JArray(propertyInfo.EditValue);
                }
                else if (propertyInfo.FieldType == FieldType.Json)
                {
                    token = JsonConvert.DeserializeObject(propertyInfo.EditValue);
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