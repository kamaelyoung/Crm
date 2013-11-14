using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core.Organization;

namespace Coldew.Core
{
    public class CreatedTimeField : DateField
    {
        public CreatedTimeField(FieldNewInfo info)
            : base(info, false)
        {
            
        }
    }
}
