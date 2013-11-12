using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Coldew.Core
{
    public class JsonMetadataValue : MetadataValue
    {
        public JsonMetadataValue(JToken value, Field field)
            : base(value, field)
        {

        }

        public override JToken PersistenceValue
        {
            get { return this.Value; }
        }

        public override string ShowValue
        {
            get { return JsonConvert.SerializeObject(this.Value); }
        }

        public override dynamic OrderValue
        {
            get { return ""; }
        }

        public override dynamic EditValue
        {
            get { return JsonConvert.SerializeObject(this.Value); }
        }
    }
}
