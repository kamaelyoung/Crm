using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Api;
using Crm.Data;
using Newtonsoft.Json;

namespace Crm.Core.Extend
{
    public class NumberFieldConfig : FieldConfig
    {
        public NumberFieldConfig(decimal? defaultValue, decimal? max, decimal? min, int precision)
        {
            this.DefaultValue = defaultValue;
            this.Max = max;
            this.Min = min;
            this.Precision = precision;
        }

        public decimal? DefaultValue { set; get; }

        public decimal? Max { set; get; }

        public decimal? Min { set; get; }

        public int Precision { set; get; }

        public override PropertyValueType ValueType
        {
            get { return PropertyValueType.Number; }
        }

        public override FieldConfigInfo Map()
        {
            NumberFieldConfigInfo info = new NumberFieldConfigInfo
            {
                DefaultValue = this.DefaultValue,
                Max = this.Max,
                Min = this.Min,
                Precision = this.Precision
            };
            return info;
        }

        public override string PersistenceValue
        {
            get
            {
                NumberFieldConfigModel model = new NumberFieldConfigModel { DefaultValue = this.DefaultValue, Max = this.Max, Min = this.Min, Precision = this.Precision };
                return JsonConvert.SerializeObject(model);
            }
        }
    }
}
