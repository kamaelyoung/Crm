using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Coldew.Data;
using Coldew.Api;

namespace Coldew.Core
{
    public class MetadataPropertyListHelper
    {
        public static string ToPropertyModelJson(List<MetadataProperty> propertys)
        {
            List<MetadataPropertyModel> propertyModels = propertys.Select(x => new MetadataPropertyModel { FieldId = x.Field.ID, Value = x.Value.PersistenceValue }).ToList();
            return JsonConvert.SerializeObject(propertyModels);
        }

        public static List<MetadataProperty> MapPropertys(PropertySettingDictionary dictionary, ColdewObject form)
        {
            List<MetadataProperty> propertys = new List<MetadataProperty>();
            foreach (KeyValuePair<string, string> pair in dictionary)
            {
                Field field = form.GetFieldByCode(pair.Key);
                MetadataValue metadataValue = field.CreateMetadataValue(pair.Value);
                propertys.Add(new MetadataProperty(metadataValue));
            }

            return propertys;
        }

        public static List<MetadataProperty> GetPropertys(string propertysJson, ColdewObject form)
        {
            List<MetadataPropertyModel> propertyModels = JsonConvert.DeserializeObject<List<MetadataPropertyModel>>(propertysJson);
            List<MetadataProperty> propertys = new List<MetadataProperty>();
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
