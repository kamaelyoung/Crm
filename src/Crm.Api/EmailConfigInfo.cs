using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm.Api
{
    [Serializable]
    public class EmailConfigInfo
    {
        public string Account { set; get; }
        public string Name { set; get; }
        public string Address { set; get; }
        public string Password { set; get; }
        public string Server { set; get; }
    }
}
