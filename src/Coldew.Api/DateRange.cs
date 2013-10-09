using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Api
{
    [Serializable]
    public class DateRange
    {
        public DateRange()
        {
        }

        public DateRange(DateTime? start, DateTime? end)
        {
            this.StartDate = start;
            this.EndDate = end;
        }

        public DateTime? StartDate { set; get; }

        public DateTime? EndDate { set; get; }

        public bool InRange(DateTime? date)
        {
            if (!date.HasValue)
            {
                return false;
            }

            if (this.EndDate.HasValue && this.StartDate.HasValue)
            {
                if (date < StartDate || date > this.EndDate)
                {
                    return false;
                }
            }
            else if (this.EndDate.HasValue)
            {
                if (date > this.EndDate)
                {
                    return false;
                }
            }
            else if (this.StartDate.HasValue)
            {
                if (date < this.StartDate)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
