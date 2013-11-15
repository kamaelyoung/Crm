using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Api
{
    [Serializable]
    public class UserMetadataInfo : MetadataInfo
    {
        public bool Favorited { set; get; }

        public MetadataPermissionValue PermissionValue { set; get; }
    }
}
