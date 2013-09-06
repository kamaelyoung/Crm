using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Api;
using Newtonsoft.Json;

namespace Crm.Core.Extend
{
    public class CheckboxFieldConfig : FieldConfig
    {
        public CheckboxFieldConfig(List<string> defaultValues, List<string> selectList)
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

        public List<string> DefaultValues { set; get; }

        public List<string> SelectList { set; get; }

        public override FieldConfigInfo Map()
        {
            CheckboxFieldConfigInfo fieldInfo = new CheckboxFieldConfigInfo
            {
                DefaultValues = this.DefaultValues,
                SelectList = this.SelectList,
            };
            return fieldInfo;
        }

        public override PropertyValueType ValueType
        {
            get { return PropertyValueType.StringList; }
        }

        public override string PersistenceValue
        {
            get 
            {
                CheckboxFieldConfigJsonModel model = new CheckboxFieldConfigJsonModel { DefaultValues = this.DefaultValues, SelectList = this.SelectList };
                return JsonConvert.SerializeObject(model); 
            }
        }

    }
}
