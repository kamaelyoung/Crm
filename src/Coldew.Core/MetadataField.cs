using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api;

namespace Coldew.Core
{
    public class MetadataField: Field
    {
        public MetadataField(FieldNewInfo info, Form valueForm)
            :base(info)
        {
            this.ValueForm = valueForm;
        }

        public Form ValueForm { private set;get;}

        public override string Type
        {
            get { return FieldType.Metadata; }
        }

        public override string TypeName
        {
            get { return this.ValueForm.Name; }
        }

        public override MetadataValue CreateMetadataValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("value");
            }
            return new MetadataMetadataValue(value, this);
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
