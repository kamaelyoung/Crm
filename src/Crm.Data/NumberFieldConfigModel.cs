using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm.Data
{
    public class NumberFieldConfigModel
    {
        public decimal? DefaultValue { set; get; }

        public decimal? Max { set; get; }

        public decimal? Min { set; get; }

        public int Precision { set; get; }
    }
}
