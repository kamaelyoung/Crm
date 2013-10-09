using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Coldew.Data;
using Coldew.Api;

namespace Coldew.Core
{
    public class MetadataPropertyList : List<MetadataProperty>
    {
        public string ToJson()
        {
            List<MetadataPropertyModel> propertyModels = this.Select(x => new MetadataPropertyModel { FieldId = x.Field.ID, Value = x.Value.PersistenceValue }).ToList();
            return JsonConvert.SerializeObject(propertyModels);
        }

        public static MetadataPropertyList MapPropertys(PropertySettingDictionary dictionary, Form form)
        {
            MetadataPropertyList propertys = new MetadataPropertyList();
            foreach (KeyValuePair<string, string> pair in dictionary)
            {
                Field field = form.GetFieldByCode(pair.Key);
                MetadataValue metadataValue = field.CreateMetadataValue(pair.Value);
                propertys.Add(new MetadataProperty(metadataValue));
            }

            return propertys;
        }

        public static MetadataPropertyList GetPropertys(string propertysJson, Form form)
        {
            List<MetadataPropertyModel> propertyModels = JsonConvert.DeserializeObject<List<MetadataPropertyModel>>(propertysJson);
            MetadataPropertyList propertys = new MetadataPropertyList();
            foreach (MetadataPropertyModel propertyModel in propertyModels)
            {
                Field field = form.GetFieldById(propertyModel.FieldId);
                if (field != null)
                {
                    MetadataValue metadataValue = field.CreateMetadataValue(propertyModel.Value);
                    propertys.Add(new MetadataProperty(metadataValue));
                }
            }
            return propertys;
        }
    }
}
