using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm.Api
{
    [Serializable]
    public class DateFieldConfigInfo : FieldConfigInfo
    {
        public bool DefaultValueIsToday { set; get; }

        public string DefaultValue
        {
            get
            {
                if (this.DefaultValueIsToday)
                {
                    return DateTime.Now.ToString("yyyy-MM-dd");
                }
                return "";
            }
        }
    }
}
