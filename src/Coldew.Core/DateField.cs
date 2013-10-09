using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api;
using Coldew.Data;
using Newtonsoft.Json;
using Coldew.Api.Exceptions;

namespace Coldew.Core
{
    public class DateField : Field
    {
        public DateField(FieldNewInfo info, bool defaultValueIsToday)
            :base(info)
        {
            this.DefaultValueIsToday = defaultValueIsToday;
        }

        public override string Type
        {
            get { return FieldType.Date; }
        }

        public override string TypeName
        {
            get { return "日期"; }
        }

        public bool DefaultValueIsToday { set; get; }

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

        public override MetadataValue CreateMetadataValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return new DateMetadataValue(null, this);
            }
            DateTime date;
            if (DateTime.TryParse(value, out date))
            {
                return new DateMetadataValue(date, this);
            }
            else
            {
                throw new ColdewException("创建DateMetadataValue出错,value:" + value);
            }
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
