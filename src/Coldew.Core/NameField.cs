using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api;

namespace Coldew.Core
{
    public class NameField : StringField
    {
        public NameField(FieldNewInfo info)
            :base(info, "")
        {
            
        }
    }
}
