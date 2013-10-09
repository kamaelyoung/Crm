using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Api
{
    [Serializable]
    public class UserListFieldInfo : FieldInfo
    {

        public bool DefaultValueIsCurrent { set; get; }
    }
}
