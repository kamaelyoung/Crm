using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api;
using Coldew.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Coldew.Core
{
    public class CheckboxListField : Field
    {
        public CheckboxListField(FieldNewInfo info, List<string> defaultValues, List<string> selectList)
            :base(info)
        {
            if (defaultValues == null)
            {
                defaultValues = new List<string>();
            }
            this.DefaultValues = defaultValues;
            if (selectList == null)
            {
                selectList = new List<string>();
            }
            this.SelectList = selectList;
        }

        public override string TypeName
        {
            get { return "复选框"; }
        }

        public List<string> DefaultValues { set; get; }

        public List<string> SelectList { set; get; }

        public override MetadataValue CreateMetadataValue(JToken value)
        {
            List<string> stringList = new List<string>();
            if (value != null)
            {
                foreach (JToken str in value)
                {
                    stringList.Add(str.ToString());
                }
            }
            return new StringListMetadataValue(stringList, this);
        }

        public void Modify(string name, bool required, List<string> defaultValues, List<string> selectList)
        {
            FieldModifyArgs args = new FieldModifyArgs { Name = name, Required = required };
            this.OnModifying(args);

            FieldModel model = NHibernateHelper.CurrentSession.Get<FieldModel>(this.ID);
            model.Name = name;
            model.Required = required;
            CheckboxFieldConfigModel configModel = new CheckboxFieldConfigModel { DefaultValues = defaultValues, SelectList = selectList };
            model.Config = JsonConvert.SerializeObject(configModel);

            NHibernateHelper.CurrentSession.Update(model);
            NHibernateHelper.CurrentSession.Flush();

            this.Name = name;
            this.Required = required;
            this.DefaultValues = defaultValues;
            this.SelectList = selectList;

            this.OnModifyed(args);
        }

        public override FieldInfo Map()
        {
            CheckboxFieldInfo info = new CheckboxFieldInfo();
            info.DefaultValues = this.DefaultValues;
            info.SelectList = this.SelectList;
            this.Fill(info);
            return info;
        }
    }
}
