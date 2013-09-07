using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Api;
using Newtonsoft.Json;
using Crm.Data;

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
                    ListFieldConfigModel listFieldConfig = JsonConvert.DeserializeObject<ListFieldConfigModel>(config);
                    return new ListFieldConfig(listFieldConfig.DefaultValue, listFieldConfig.SelectList);
                case FieldType.CheckboxList:
                    CheckboxFieldConfigModel checkboxFieldConfig = JsonConvert.DeserializeObject<CheckboxFieldConfigModel>(config);
                    return new CheckboxFieldConfig(checkboxFieldConfig.DefaultValues, checkboxFieldConfig.SelectList);
                case FieldType.Number:
                    NumberFieldConfigModel numberFieldConfigModel = JsonConvert.DeserializeObject<NumberFieldConfigModel>(config);
                    return new NumberFieldConfig(numberFieldConfigModel.DefaultValue, numberFieldConfigModel.Max, numberFieldConfigModel.Min, numberFieldConfigModel.Precision);
                case FieldType.Date:
                    DateFieldConfigModel dateFieldConfigModel = JsonConvert.DeserializeObject<DateFieldConfigModel>(config);
                    return new DateFieldConfig(dateFieldConfigModel.DefaultValueIsToday);
            }
            throw new ArgumentException("type");
        }

        public abstract FieldConfigInfo Map();
    }
}
