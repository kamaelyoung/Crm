using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Coldew.Data;
using Coldew.Api;
using Newtonsoft.Json.Linq;

namespace Coldew.Core
{
    public class MetadataPropertyListHelper
    {
        public static string ToPropertyModelJson(List<MetadataProperty> propertys)
        {
            JObject jobject = new JObject();
            foreach (MetadataProperty property in propertys)
            {
                jobject.Add(property.Field.Code, property.Value.PersistenceValue);
            }
            return JsonConvert.SerializeObject(jobject);
        }

        public static List<MetadataProperty> MapPropertys(JObject jobject, ColdewObject form)
        {
            List<MetadataProperty> propertys = new List<MetadataProperty>();
            foreach (JProperty property in jobject.Properties())
            {
                Field field = form.GetFieldByCode(property.Name);
                if (field != null)
                {
                    MetadataValue metadataValue = field.CreateMetadataValue(property.Value);
                    propertys.Add(new MetadataProperty(metadataValue));
                }
            }

            return propertys;
        }
    }
}
