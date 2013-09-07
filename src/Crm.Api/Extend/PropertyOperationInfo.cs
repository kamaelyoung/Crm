using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm.Api
{
    [Serializable]
    public class PropertyOperationInfo
    {
        public string Code { set; get; }

        string _value;
        public string Value
        {
            set
            {
                if (value != null)
                {
                    _value = value.Trim();
                }
            }
            get
            {
                return _value;
            }
        }
    }
}
