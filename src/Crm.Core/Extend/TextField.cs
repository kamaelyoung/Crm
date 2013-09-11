using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm.Core.Extend
{
    public class TextField : StringField
    {
        public TextField(FieldNewInfo info, string defaultValue)
            : base(info, defaultValue)
        {
            
        }
    }
}
