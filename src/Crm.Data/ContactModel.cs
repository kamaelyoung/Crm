using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm.Data
{
    public class ContactModel
    {
        public virtual string ID { set; get; }

        public virtual string Name { set; get; }

        public virtual string CustomerId { set; get; }

        public virtual string CreatorId { set; get; }

        public virtual DateTime CreateTime { set; get; }

        public virtual string ModifiedUserId { set; get; }

        public virtual DateTime ModifiedTime { set; get; }

        public virtual int MetadataId { set; get; }
    }
}
