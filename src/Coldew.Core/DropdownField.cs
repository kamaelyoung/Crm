using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api;

namespace Coldew.Core
{
    public class DropdownField : ListField
    {
        public DropdownField(FieldNewInfo info, string defaultValue, List<string> selectList)
            :base(info, defaultValue, selectList)
        {

        }

        public override string TypeName
        {
            get { return "下拉选项"; }
        }
    }
}
