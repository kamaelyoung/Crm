using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm.Core.Extend
{
    public class RadioListField : ListField
    {
        public RadioListField(FieldNewInfo info, string defaultValue, List<string> selectList)
            :base(info, defaultValue, selectList)
        {

        }
    }
}
