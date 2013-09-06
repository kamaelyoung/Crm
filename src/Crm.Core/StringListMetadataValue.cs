using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Api;

namespace Crm.Core
{
    public class StringListMetadataValue : MetadataValue
    {
        public StringListMetadataValue(List<string> value)
            :base(value)
        {

        }

        public List<string> StringList
        {
            get
            {
                return this.Value;
            }
        }

        public override string PersistenceValue
        {
            get { return string.Join(",", this.StringList); }
        }

        public override string ShowValue
        {
            get { return string.Join(",", this.StringList); }
        }

        public override PropertyValueType Type
        {
            get { return PropertyValueType.StringList; }
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
