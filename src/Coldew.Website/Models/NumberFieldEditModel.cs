using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coldew.Api;

namespace Coldew.Website.Models
{
    public class NumberFieldEditModel
    {
        public NumberFieldEditModel(NumberFieldInfo field)
        {
            this.id = field.ID;
            this.name = field.Name;
            this.required = field.Required;
            this.defaultValue = field.DefaultValue;
            this.max = field.Max;
            this.min = field.Min;
            this.precision = field.Precision;
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