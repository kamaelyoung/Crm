using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Api;

namespace Crm.Core
{
    public class StringMetadataValue : MetadataValue
    {
        public StringMetadataValue(string value)
            :base(value)
        {

        }

        public string String
        {
            get
            {
                return this.Value;
            }
        }

        public override string PersistenceValue
        {
            get { return this.String; }
        }

        public override string ShowValue
        {
            get { return this.String; }
        }

        public override PropertyValueType Type
        {
            get { return PropertyValueType.String; }
        }

        public override dynamic OrderValue
        {
            get { return this.String; }
        }

        public override dynamic EditValue
        {
            get { return this.Value; }
        }
    }
}
