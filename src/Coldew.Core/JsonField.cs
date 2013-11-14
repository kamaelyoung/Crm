using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api;
using Newtonsoft.Json.Linq;
using Coldew.Data;

namespace Coldew.Core
{
    public class JsonField : Field
    {
        public JsonField(FieldNewInfo info)
            :base(info)
        {
            
        }

        public override string TypeName
        {
            get { return "Json"; }
        }

        public string DefaultValue { set; get; }

        public override MetadataValue CreateMetadataValue(JToken value)
        {
            return new JsonMetadataValue(value, this);
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
