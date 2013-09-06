using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Core.Organization;

namespace Crm.Core
{
    public class CustomerAreaModifyInfo
    {
        string _name;
        public string Name
        {
            set
            {
                if (value != null)
                {
                    _name = value.Trim();
                }
            }
            get
            {
                return _name;
            }
        }

        public List<User> ManagerUsers { set; get; }
    }
}
