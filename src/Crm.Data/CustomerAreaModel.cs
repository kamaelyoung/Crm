using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm.Data
{
    public class CustomerAreaModel
    {
        public virtual int ID { set; get; }

        public virtual string Name { set; get; }

        public virtual string ManagerAccounts { set; get; }
    }
}
