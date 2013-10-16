using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Coldew.Api;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Coldew.Core
{
    public abstract class MetadataSearcher
    {
        public abstract bool Accord(Metadata metadata);
    }
}
