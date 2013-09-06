using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Crm.Api;

namespace Crm.Website.Models
{
    public class ListFieldEditModel
    {
        public ListFieldEditModel(FieldInfo field, ListFieldConfigInfo configInfo)
        {
            this.id = field.ID;
            this.name = field.Name;
            this.required = field.Required;
            this.defaultValue = configInfo.DefaultValue;
            this.index = field.Index;
            this.selectList = string.Join(",", configInfo.SelectList);
        }

        public ListFieldEditModel(FieldInfo field, CheckboxFieldConfigInfo configInfo)
        {
            this.id = field.ID;
            this.name = field.Name;
            this.required = field.Required;
            this.defaultValue = string.Join(",", configInfo.DefaultValues);
            this.index = field.Index;
            this.selectList = string.Join(",", configInfo.SelectList);
        }

        public int id;

        public string name;

        public bool required;

        public string selectList;

        public string defaultValue;

        public int index;
    }
}