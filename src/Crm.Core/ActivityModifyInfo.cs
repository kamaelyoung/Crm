using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Core.Organization;
using Crm.Api;

namespace Crm.Core
{
    public class ActivityModifyInfo
    {
        public User OpUser {set;get;}
        string _subject;
        public string Subject
        {
            set
            {
                if (value != null)
                {
                    _subject = value.Trim();
                }
            }
            get
            {
                return _subject;
            }
        }
        public List<PropertyOperationInfo> PropertyInfos { set; get; } 
    }
}
