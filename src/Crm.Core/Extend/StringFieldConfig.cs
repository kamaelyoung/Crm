using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Api;
using Crm.Data;
using Crm.Core.Organization;

namespace Crm.Core.Extend
{
    public class StringFieldConfig : FieldConfig
    {
        public StringFieldConfig(string defaultValue)
        {
            this.DefaultValue = defaultValue;
        }

        public string DefaultValue { set; get; }

        public override PropertyValueType ValueType
        {
            get { return PropertyValueType.String; }
        }

        public void Modify(string defautlValue)
        {
            FieldModel model = NHibernateHelper.CurrentSession.Get<FieldModel>(this.Field.ID);
            model.Config = defautlValue;

            NHibernateHelper.CurrentSession.Update(model);
            NHibernateHelper.CurrentSession.Flush();

            this.DefaultValue = defautlValue;
        }

        public override FieldConfigInfo Map()
        {
            StringFieldConfigInfo info = new StringFieldConfigInfo
            {
                DefaultValue = this.DefaultValue
            };
            return info;
        }

        public override string PersistenceValue
        {
            get { return this.DefaultValue; }
        }
    }
}
