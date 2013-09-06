using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Core.Organization;
using Crm.Api;

namespace Crm.Core
{
    public class ContactModifyInfo
    {
        public User OpUser {set;get;}
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
        public List<PropertyOperationInfo> PropertyInfos { set; get; } 
    }
}
