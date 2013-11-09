using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api.Exceptions;
using Coldew.Api;
using Coldew.Core.Organization;

namespace Coldew.Core.Search
{
    public class DateRecentlySearchExpression : SearchExpression
    {
        int? _startDays;
        int? _endDays;
        public DateRecentlySearchExpression(Field field, int? startDays, int? endDays)
            :base(field)
        {
            this._startDays = startDays;
            this._endDays = endDays;
        }

        public override bool Compare(User opUser, Metadata metadata)
        {
            MetadataProperty property = metadata.GetProperty(this.Field.Code);
            if (property != null)
            {
                if (!(property.Value is DateMetadataValue))
                {
                    throw new ColdewException(string.Format("{0} 不是日期类型字段, 无法执行搜索", this.Field.Name));
                }

                DateMetadataValue value = property.Value as DateMetadataValue;
                DateTime? startDate = null;
                DateTime? endDate = null;
                if (this._startDays.HasValue)
                {
                    startDate = DateTime.Now.AddDays(this._startDays.Value);
                }
                if (this._endDays.HasValue)
                {
                    endDate = DateTime.Now.AddDays(this._endDays.Value);
                }
                DateRange range = new DateRange(startDate, endDate);
                return range.InRange(value.Date);
            }

            return true;
        }
    }
}
