using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Core.Workflow
{
    class Helper
    {
        public static bool InDateRange(DateTime date, DateTime? startDate, DateTime? endDate)
        {
            if (!startDate.HasValue && !endDate.HasValue)
            {
                return true;
            }

            if (endDate.HasValue && startDate.HasValue)
            {
                if (date < startDate || date > endDate)
                {
                    return false;
                }
            }
            else if (endDate.HasValue)
            {
                if (date > endDate)
                {
                    return false;
                }
            }
            else if (startDate.HasValue)
            {
                if (date < startDate)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            return true;
        }
    }
}
