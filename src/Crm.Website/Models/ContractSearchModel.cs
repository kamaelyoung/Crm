using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Crm.Api;
using System.Text.RegularExpressions;

namespace Crm.Website.Models
{
    public class ContractSearchModel
    {
        public string keyword;
        public DateTime? startDateRangeStart;
        public DateTime? startDateRangeEnd;
        public DateTime? endDateRangeStart;
        public DateTime? endDateRangeEnd;
        public float? valueRangeMin;
        public float? valueRangeMax;
        public int start;
        public int size;

        public ContractSearchInfo Map()
        {
            DateRange startDateRange = null;
            if(this.startDateRangeStart.HasValue || this.startDateRangeEnd.HasValue)
            {
                startDateRange = new DateRange{ EndDate = startDateRangeEnd, StartDate = startDateRangeStart};
            }
            DateRange endDateRange = null;
            if(this.endDateRangeStart.HasValue || this.endDateRangeEnd.HasValue)
            {
                endDateRange = new DateRange{ EndDate = endDateRangeEnd, StartDate = endDateRangeStart};
            }
            FloagRange valueRange = null;
            if(this.valueRangeMin.HasValue || this.valueRangeMax.HasValue)
            {
                valueRange = new FloagRange{ Max = valueRangeMax, Min = valueRangeMin};
            }
            List<string> keywords = null;
            if (!string.IsNullOrEmpty(this.keyword))
            {
                Regex regex = new Regex("\\s+");
                this.keyword = regex.Replace(this.keyword, " ");
                keywords = keyword.Split(' ').ToList();
            }

            if (endDateRange != null || keywords != null || startDateRange != null || valueRange != null)
            {
                return new ContractSearchInfo
                {
                    EndDateRange = endDateRange,
                    Keywords = keywords,
                    StartDateRange = startDateRange,
                    ValueRange = valueRange
                };
            }

            return null;
        }
    }
}