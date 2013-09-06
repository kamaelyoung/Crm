using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm.Data
{
    public class GridViewColumnModel
    {
        public virtual int ID { set; get; }

        public virtual int FieldId { set; get; }

        public virtual int ViewId { set; get; }

        public virtual int Width { set; get; }
    }
}
