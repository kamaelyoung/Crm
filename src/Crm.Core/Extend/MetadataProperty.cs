using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Api;

namespace Crm.Core.Extend
{
    public class MetadataProperty
    {
        public MetadataProperty(Field field, MetadataValue value)
        {
            this.Field = field;
            this.Value = value;
        }

        public Field Field { private set; get; }

        public MetadataValue Value { private set; get; }

        public PropertyInfo Map()
        {
            return new PropertyInfo
            {
                Code = this.Field.Code,
                Value = this.Value.ShowValue,
                EditValue = this.Value.EditValue
            };
        }
    }
}
