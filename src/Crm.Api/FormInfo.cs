using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm.Api
{
    [Serializable]
    public class FormInfo
    {
        public int ID { set; get; }

        public string Name { set; get; }

        public FormType Type { set; get; }
    }
}
