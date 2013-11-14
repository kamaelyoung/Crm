using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Data
{
    public class FieldModel
    {
        public virtual int ID { set; get; }

        public virtual string ObjectId { set; get; }

        public virtual string Code { set; get; }

        public virtual string Name { set; get; }

        public virtual string Tip { set; get; }

        public virtual bool Required { set; get; }

        public virtual bool IsSystem { set; get; }

        public virtual string Type { set; get; }

        public virtual string Config { set; get; }
    }
}
