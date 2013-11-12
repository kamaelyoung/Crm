using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Data
{
    public class MetadataPermissionStrategyModel
    {
        public virtual string ID { set; get; }

        public virtual string ObjectId { set; get; }

        public virtual string Member { set; get; }

        public virtual int Value { set; get; }

        public virtual string SearchExpressions { set; get; }
    }
}
