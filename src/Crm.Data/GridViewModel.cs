using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm.Data
{
    public class GridViewModel
    {
        public virtual int ID { set; get; }

        public virtual string CreatorAccount { set; get; }

        public virtual bool IsSystem { set; get; }

        public virtual int Type {  set; get; }
    }
}
