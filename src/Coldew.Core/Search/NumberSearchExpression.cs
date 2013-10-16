
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api.Exceptions;
using Coldew.Api;

namespace Coldew.Core
{
    public class NumberSearchExpression : SearchExpression
    {
        NumberRange _range;
        public NumberSearchExpression(Field field, decimal? min, decimal? max)
            :base(field)
        {
            this._range = new NumberRange(min, max);
        }

        public override bool Compare(Metadata metadata)
        {
            MetadataProperty property = metadata.GetProperty(this.Field.Code);
            if (property != null)
            {
                if (!(property.Value is NumberMetadataValue))
                {
                    throw new ColdewException(string.Format("{0} 不是数字类型字段, 无法执行搜索", this.Field.Name));
                }

                NumberMetadataValue value = property.Value as NumberMetadataValue;
                return this._range.InRange(value.Number);
            }

            return true;
        }
    }
}
