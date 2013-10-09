using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Api.Organization
{
    [Serializable]
    public class FunctionCreateInfo
    {
        public string ID { set; get; }

        public string Name { set; get; }

        public string ParentId { set; get; }

        public string Url { set; get; }

        public string IconClass { set; get; }

        public int Sort { set; get; }
    }
}
