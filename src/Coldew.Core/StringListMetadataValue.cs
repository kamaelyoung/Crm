using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Coldew.Core
{
    public class StringListMetadataValue : MetadataValue
    {
        public StringListMetadataValue(List<string> value, Field field)
            :base(value, field)
        {

        }

        public List<string> StringList
        {
            get
            {
                return this.Value;
            }
        }

        public override JToken PersistenceValue
        {
            get
            {
                JArray jarray = new JArray();
                if (this.StringList != null)
                {
                    foreach (string str in this.StringList)
                    {
                        jarray.Add(str);
                    }
                }
                return jarray;
            }
        }

        public override string ShowValue
        {
            get { return string.Join(",", this.StringList); }
        }

        public override dynamic OrderValue
        {
            get { return string.Join(",", this.StringList); }
        }

        public override dynamic EditValue
        {
            get { return this.Value; }
        }
    }
}
