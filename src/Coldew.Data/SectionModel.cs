using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Data
{
    public class SectionModel
    {
        public virtual string Title { set; get; }

        public virtual int ColumnCount { set; get; }

        public virtual List<InputModel> Inputs { set; get; }
    }
}
