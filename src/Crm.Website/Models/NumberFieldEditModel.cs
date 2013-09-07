using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Crm.Api;

namespace Crm.Website.Models
{
    public class NumberFieldEditModel
    {
        public NumberFieldEditModel(FieldInfo field, NumberFieldConfigInfo configInfo)
        {
            this.id = field.ID;
            this.name = field.Name;
            this.required = field.Required;
            this.defaultValue = configInfo.DefaultValue;
            this.max = configInfo.Max;
            this.min = configInfo.Min;
            this.precision = configInfo.Precision;
            this.index = field.Index;
        }

        public int id;

        public string name;

        public bool required;

        public decimal? defaultValue { set; get; }

        public decimal? max { set; get; }

        public decimal? min { set; get; }

        public int precision { set; get; }

        public int index;
    }
}