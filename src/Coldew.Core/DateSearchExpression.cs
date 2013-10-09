
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api.Exceptions;
using Coldew.Api;

namespace Coldew.Core
{
    public class DateSearchExpression : SearchExpression
    {
        DateRange _range;
        public DateSearchExpression(Field field, DateTime? start, DateTime? end)
            :base(field)
        {
            this._range = new DateRange(start, end);
        }

        public override bool Compare(Metadata metadata)
        {
            MetadataProperty property = metadata.GetProperty(this._field.Code);
            if (property != null)
            {
                if (!(property.Value is DateMetadataValue))
                {
                    throw new ColdewException(string.Format("{0} 不是日期类型字段, 无法执行搜索", this._field.Name));
                }

                DateMetadataValue value = property.Value as DateMetadataValue;
                return this._range.InRange(value.Date);
            }

            return true;
        }
    }
}
