using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Api;
using Crm.Data;
using Newtonsoft.Json;

namespace Crm.Core.Extend
{
    public class DateField : Field
    {
        public DateField(FieldNewInfo info, bool defaultValueIsToday)
            :base(info)
        {
            this.DefaultValueIsToday = defaultValueIsToday;
        }

        public bool DefaultValueIsToday { set; get; }

        public override PropertyValueType ValueType
        {
            get { return PropertyValueType.Date; }
        }

        public void Modify(string name, bool required, bool defaultValueIsToday, int index)
        {
            FieldModifyArgs args = new FieldModifyArgs { Name = name, Required = required, Index = index };

            this.OnModifying(args);

            FieldModel model = NHibernateHelper.CurrentSession.Get<FieldModel>(this.ID);
            model.Name = name;
            model.Required = required;
            model.Index = index;
            DateFieldConfigModel configModel = new DateFieldConfigModel { DefaultValueIsToday = defaultValueIsToday };
            model.Config = JsonConvert.SerializeObject(configModel);

            NHibernateHelper.CurrentSession.Update(model);
            NHibernateHelper.CurrentSession.Flush();

            this.Name = name;
            this.Index = index;
            this.Required = required;
            this.DefaultValueIsToday = defaultValueIsToday;

            this.OnModifyed(args);
        }

        public override FieldInfo Map()
        {
            DateFieldInfo info = new DateFieldInfo();
            info.DefaultValueIsToday = this.DefaultValueIsToday;
            this.Fill(info);
            return info;
        }
    }
}
