using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api;

namespace Coldew.Core
{
    public class StringMetadataValue : MetadataValue
    {
        public StringMetadataValue(string value, Field field)
            :base(value, field)
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
