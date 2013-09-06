using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Api;
using Newtonsoft.Json;

namespace Crm.Core.Extend
{
    public abstract class FieldConfig
    {
        internal Field Field { set; get; }

        public abstract string PersistenceValue { get; }

        public abstract PropertyValueType ValueType { get; }

        public static FieldConfig CreateFieldConfig(FieldType type, string config)
        {
            switch (type)
            {
                case FieldType.String:
                case FieldType.Text:
                case FieldType.System:
                    return new StringFieldConfig(config);
                case FieldType.DropdownList:
                case FieldType.RadioList:
                    ListFieldConfigJsonModel listFieldConfig = JsonConvert.DeserializeObject<ListFieldConfigJsonModel>(config);
                    return new ListFieldConfig(listFieldConfig.DefaultValue, listFieldConfig.SelectList);
                case FieldType.CheckboxList:
                    CheckboxFieldConfigJsonModel checkboxFieldConfig = JsonConvert.DeserializeObject<CheckboxFieldConfigJsonModel>(config);
                    return new CheckboxFieldConfig(checkboxFieldConfig.DefaultValues, checkboxFieldConfig.SelectList);
            }
            throw new ArgumentException("type");
        }

        public abstract FieldConfigInfo Map();
    }
}
