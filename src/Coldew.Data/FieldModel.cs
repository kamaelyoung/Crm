using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Data
{
    public class FieldModel
    {
        public virtual int ID { set; get; }

        public virtual string FormId { set; get; }

        public virtual string Code { set; get; }

        public virtual string Name { set; get; }

        public virtual bool Required { set; get; }

        public virtual bool CanModify { set; get; }

        public virtual bool CanInput { set; get; }

        public virtual int Index { set; get; }

        public virtual string Type { set; get; }

        public virtual string Config { set; get; }
    }
}
