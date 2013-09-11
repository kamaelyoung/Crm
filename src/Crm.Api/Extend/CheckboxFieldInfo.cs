using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm.Api
{
    [Serializable]
    public class CheckboxFieldInfo : FieldInfo
    {
        public List<string> DefaultValues { set; get; }

        public List<string> SelectList { set; get; }
    }
}
