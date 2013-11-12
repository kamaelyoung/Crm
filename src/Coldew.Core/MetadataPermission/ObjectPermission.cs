using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core.Organization;

namespace Coldew.Core.MetadataPermission
{
    public class ObjectPermission
    {
        public string ID{ private set; get; }

        public Member Member { private set; get; }

        public string ObjectId { private set; get; }
    }
}
