using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Data
{
    public class MetadataFavoriteModel
    {
        public virtual int ID { set; get; }

        public virtual string FormId { set; get; }

        public virtual string UserId { set; get; }

        public virtual string MetadataId { set; get; }
    }
}
