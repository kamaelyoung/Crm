using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Core.Organization;
using Crm.Api;

namespace Crm.Core
{
    public class ContractModifyInfo
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

        public DateTime StartDate { set; get; }

        public DateTime EndDate { set; get; }

        public int ExpiredComputeDays { set; get; }

        public List<User> Owners { set; get; }

        public float Value { set; get; }

        public List<PropertyOperationInfo> PropertyInfos { set; get; } 
    }
}
