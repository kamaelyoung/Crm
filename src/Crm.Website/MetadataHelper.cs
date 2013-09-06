using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Crm.Api;
using Newtonsoft.Json.Linq;

namespace Crm.Website
{
    public class MetadataHelper
    {
        public static List<PropertyOperationInfo> MapPropertyOperationInfos(JObject extends)
        {
            List<PropertyOperationInfo> propertys = new List<PropertyOperationInfo>();

            foreach (JProperty property in extends.Properties())
            {
                if (property.Value is JArray)
                {
                    JArray array = property.Value as JArray;
                    string value = string.Join(",", array.Select(x => (string)x));
                    propertys.Add(new PropertyOperationInfo { Code = property.Name, Value = value });
                }
                else
                {
                    propertys.Add(new PropertyOperationInfo { Code = property.Name, Value = property.Value.ToString() });
                }
            }

            return propertys;
        }
    }
}