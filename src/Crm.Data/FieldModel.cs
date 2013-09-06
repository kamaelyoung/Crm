using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm.Data
{
    public class FieldModel
    {
        public virtual int ID { set; get; }

        public virtual int FormId { set; get; }

        public virtual string Code { set; get; }

        public virtual string Name { set; get; }

        public virtual bool Required { set; get; }

        public virtual bool CanModify { set; get; }

        public virtual bool CanImport { set; get; }

        public virtual int Index { set; get; }

        public virtual int Type { set; get; }

        public virtual string Config { set; get; }
    }
}
