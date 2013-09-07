using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm.Api
{
    public class PropertySearchConditionHelper
    {
        public static KeywordSearchCondition CreateKeywordCondition(string keyword)
        {
            return new KeywordSearchCondition { Keyword = keyword };
        }

        public static DateRange CreateDateCondition(DateTime? start, DateTime? end)
        {
            return new DateRange { StartDate = start, EndDate = end };
        }

        public static NumberRange CreateNumberCondition(decimal? min, decimal? max)
        {
            return new NumberRange { Max = max, Min = min };
        }
    }
}
