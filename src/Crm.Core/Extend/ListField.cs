using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Api;
using Crm.Data;
using Newtonsoft.Json;

namespace Crm.Core.Extend
{
    public class ListField : Field
    {
        public ListField(FieldNewInfo info, string defaultValue, List<string> selectList)
            :base(info)
        {
            this.DefaultValue = defaultValue;
            if (selectList == null)
            {
                selectList = new List<string>();
            }
            this.SelectList = selectList;
        }

        public string DefaultValue { set; get; }

        public List<string> SelectList { set; get; }

        public override PropertyValueType ValueType
        {
            get { return PropertyValueType.String; }
        }

        public void Modify(string name, bool required, string defaultValue, List<string> selectList, int index)
        {
            FieldModifyArgs args = new FieldModifyArgs { Name = name, Required = required, Index = index };

            this.OnModifying(args);

            FieldModel model = NHibernateHelper.CurrentSession.Get<FieldModel>(this.ID);
            model.Name = name;
            model.Required = required;
            model.Index = index;
            ListFieldConfigModel configModel = new ListFieldConfigModel { DefaultValue = defaultValue, SelectList = selectList };
            model.Config = JsonConvert.SerializeObject(configModel);

            NHibernateHelper.CurrentSession.Update(model);
            NHibernateHelper.CurrentSession.Flush();

            this.Name = name;
            this.Index = index;
            this.Required = required;
            this.DefaultValue = defaultValue;
            this.SelectList = selectList;

            this.OnModifyed(args);
        }

        public override FieldInfo Map()
        {
            ListFieldInfo info = new ListFieldInfo();
            info.DefaultValue = this.DefaultValue;
            info.SelectList = this.SelectList;
            this.Fill(info);
            return info;
        }
    }
}
