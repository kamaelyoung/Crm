using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Api;
using Crm.Data;
using Newtonsoft.Json;

namespace Crm.Core.Extend
{
    public class DateFieldConfig : FieldConfig
    {
        public DateFieldConfig(bool defaultValueIsToday)
        {
            this.DefaultValueIsToday = defaultValueIsToday;
        }

        public bool DefaultValueIsToday { set; get; }

        public override PropertyValueType ValueType
        {
            get { return PropertyValueType.Date; }
        }

        public override FieldConfigInfo Map()
        {
            DateFieldConfigInfo info = new DateFieldConfigInfo
            {
                DefaultValueIsToday = this.DefaultValueIsToday
            };
            return info;
        }

        public override string PersistenceValue
        {
            get
            {
                DateFieldConfigModel model = new DateFieldConfigModel { DefaultValueIsToday = this.DefaultValueIsToday};
                return JsonConvert.SerializeObject(model);
            }
        }
    }
}
