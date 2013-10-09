using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Api
{
    [Serializable]
    public class UserFieldInfo : FieldInfo
    {
        public bool DefaultValueIsCurrent { set; get; }
    }
}
