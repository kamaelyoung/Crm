using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Data
{
    public class RelatedObjectModel
    {
        public virtual List<string> FieldCodes { set; get; }

        public virtual string ObjectCode { set; get; }
    }
}
