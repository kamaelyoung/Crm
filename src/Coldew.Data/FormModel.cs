using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Data
{
    public class FormModel
    {
        public virtual string ID { set; get; }

        public virtual string Code { set; get; }

        public virtual string ObjectId { set; get; }

        public virtual string Title { set; get; }

        public virtual string SectionsJson { set; get; }

        public virtual string RelatedsJson { set; get; }
    }
}
