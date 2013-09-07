using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Api;
using Newtonsoft.Json;
using Crm.Data;

namespace Crm.Core.Extend
{
    public class ListFieldConfig : FieldConfig
    {
        public ListFieldConfig(string defaultValue, List<string> selectList)
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

        public override FieldConfigInfo Map()
        {
            ListFieldConfigInfo fieldInfo = new ListFieldConfigInfo
            {
                DefaultValue = this.DefaultValue,
                SelectList = this.SelectList,
            };
            return fieldInfo;
        }

        public override PropertyValueType ValueType
        {
            get { return PropertyValueType.String; }
        }

        public override string PersistenceValue
        {
            get 
            {
                ListFieldConfigModel model = new ListFieldConfigModel { DefaultValue = this.DefaultValue, SelectList = this.SelectList };
                return JsonConvert.SerializeObject(model); 
            }
        }

    }
}
