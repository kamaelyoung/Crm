using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api;
using Newtonsoft.Json.Linq;

namespace Coldew.Core
{
    public class MetadataField: Field
    {
        public MetadataField(FieldNewInfo info, ColdewObject valueForm)
            :base(info)
        {
            this.ValueForm = valueForm;
        }

        public ColdewObject ValueForm { private set;get;}

        public override string TypeName
        {
            get { return this.ValueForm.Name; }
        }

        public override MetadataValue CreateMetadataValue(JToken value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            return new MetadataRelatedValue(value.ToString(), this);
        }

        public override FieldInfo Map()
        {
            MetadataFieldInfo info = new MetadataFieldInfo();
            this.Fill(info);
            info.ValueFormId = this.ValueForm.ID;
            info.ValueFormName = this.ValueForm.Name;
            return info;
        }
    }
}
