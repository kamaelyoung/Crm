using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coldew.Api;
using Newtonsoft.Json.Linq;
using Coldew.Website.Models;

namespace Coldew.Website
{
    public class ExtendHelper
    {
        public static PropertySettingDictionary MapPropertySettingDictionary(JObject extends)
        {
            PropertySettingDictionary propertys = new PropertySettingDictionary();

            foreach (JProperty property in extends.Properties())
            {
                if (property.Value is JArray)
                {
                    JArray array = property.Value as JArray;
                    string value = string.Join(",", array.Select(x => (string)x));
                    propertys.Add(property.Name, value);
                }
                else
                {
                    propertys.Add(property.Name, property.Value.ToString());
                }
            }

            return propertys;
        }
    }
}