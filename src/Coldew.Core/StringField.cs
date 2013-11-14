using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api;
using Coldew.Data;
using Newtonsoft.Json.Linq;

namespace Coldew.Core
{
    public class StringField : Field
    {
        public StringField(FieldNewInfo info, string defaultValue)
            :base(info)
        {
            this.DefaultValue = defaultValue;
        }

        public override string TypeName
        {
            get { return "短文本"; }
        }

        public string DefaultValue { set; get; }

        public override MetadataValue CreateMetadataValue(JToken value)
        {
            return new StringMetadataValue(value.ToString(), this);
        }

        public void Modify(string name, bool required, string defaultValue)
        {
            FieldModifyArgs args = new FieldModifyArgs { Name = name, Required = required};

            this.OnModifying(args);

            FieldModel model = NHibernateHelper.CurrentSession.Get<FieldModel>(this.ID);
            model.Name = name;
            model.Required = required;
            model.Config = defaultValue;

            NHibernateHelper.CurrentSession.Update(model);
            NHibernateHelper.CurrentSession.Flush();

            this.Name = name;
            this.Required = required;
            this.DefaultValue = defaultValue;

            this.OnModifyed(args);
        }

        public override FieldInfo Map()
        {
            StringFieldInfo info = new StringFieldInfo();
            info.DefaultValue = this.DefaultValue;
            this.Fill(info);
            return info;
        }
    }
}
