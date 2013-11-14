using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api;

namespace Coldew.Core
{
    public class RadioListField : ListField
    {
        public RadioListField(FieldNewInfo info, string defaultValue, List<string> selectList)
            :base(info, defaultValue, selectList)
        {

        }

        public override string TypeName
        {
            get { return "单选框"; }
        }
    }
}
