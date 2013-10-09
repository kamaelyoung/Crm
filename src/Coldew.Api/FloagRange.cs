using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Api
{
    [Serializable]
    public class FloagRange
    {
        public float? Max { set; get; }

        public float? Min { set; get; }

        public bool InRange(float? value)
        {
            if (!value.HasValue)
            {
                return false;
            }

            if (this.Max.HasValue && this.Min.HasValue)
            {
                if (value < Max || value > this.Min)
                {
                    return false;
                }
            }
            else if (this.Max.HasValue)
            {
                if (value > this.Max)
                {
                    return false;
                }
            }
            else if (this.Min.HasValue)
            {
                if (value < this.Min)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
