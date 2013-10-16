using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Data
{
    public class GridViewModel
    {
        public virtual string ID { set; get; }

        public virtual string Code { set; get; }

        public virtual string Name { set; get; }

        public virtual int Type { set; get; }

        public virtual string ObjectId { set; get; }

        public virtual string CreatorAccount { set; get; }

        public virtual bool IsShared { set; get; }

        public virtual bool IsSystem { set; get; }

        public virtual int Index { set; get; }

        public virtual string ColumnsJson { set; get; }

        public virtual string SearchExpression { set; get; }
    }
}
