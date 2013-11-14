using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api;
using Coldew.Data;
using Newtonsoft.Json;
using Coldew.Api.Exceptions;
using Newtonsoft.Json.Linq;

namespace Coldew.Core
{
    public class NumberField : Field
    {
        public NumberField(FieldNewInfo info, decimal? defaultValue, decimal? max, decimal? min, int precision)
            :base(info)
        {
            this.DefaultValue = defaultValue;
            this.Max = max;
            this.Min = min;
            this.Precision = precision;
        }

        public override string TypeName
        {
            get { return "数字"; }
        }

        public decimal? DefaultValue { set; get; }

        public decimal? Max { set; get; }

        public decimal? Min { set; get; }

        public int Precision { set; get; }

        public void Modify(string name, bool required, decimal? defaultValue, decimal? max, decimal? min, int precision)
        {
            FieldModifyArgs args = new FieldModifyArgs { Name = name, Required = required};

            this.OnModifying(args);

            FieldModel model = NHibernateHelper.CurrentSession.Get<FieldModel>(this.ID);
            model.Name = name;
            model.Required = required;
            NumberFieldConfigModel configModel = new NumberFieldConfigModel { DefaultValue = defaultValue, Max = max, Min = min, Precision = precision };
            model.Config = JsonConvert.SerializeObject(configModel);

            NHibernateHelper.CurrentSession.Update(model);
            NHibernateHelper.CurrentSession.Flush();

            this.Name = name;
            this.Required = required;
            this.DefaultValue = defaultValue;
            this.Max = max;
            this.Min = min;
            this.Precision = precision;

            this.OnModifyed(args);
        }

        public override FieldInfo Map()
        {
            NumberFieldInfo info = new NumberFieldInfo();
            info.DefaultValue = this.DefaultValue;
            info.Max = this.Max;
            info.Min = this.Min;
            info.Precision = this.Precision;
            this.Fill(info);
            return info;
        }

        public override MetadataValue CreateMetadataValue(JToken value)
        {
            if (string.IsNullOrEmpty(value.ToString()))
            {
                return new NumberMetadataValue(null, this);
            }
            decimal number;
            if (decimal.TryParse(value.ToString(), out number))
            {
                return new NumberMetadataValue(number, this);
            }
            else
            {
                throw new ColdewException("创建NumberMetadataValue出错,value:" + value);
            }
        }
    }
}
