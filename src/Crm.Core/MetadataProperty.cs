using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Api;

namespace Crm.Core
{
    public class MetadataProperty
    {
        public MetadataProperty(string code, MetadataValue value)
        {
            this.Code = code;
            this.Value = value;
        }

        public string Code { private set; get; }

        public MetadataValue Value { private set; get; }

        public PropertyInfo Map()
        {
            return new PropertyInfo
            {
                Code = this.Code,
                Value = this.Value.ShowValue,
                EditValue = this.Value.EditValue
            };
        }
    }
}
