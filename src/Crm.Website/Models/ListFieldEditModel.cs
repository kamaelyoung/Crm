using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Crm.Api;

namespace Crm.Website.Models
{
    public class ListFieldEditModel
    {
        public ListFieldEditModel(ListFieldInfo field)
        {
            this.id = field.ID;
            this.name = field.Name;
            this.required = field.Required;
            this.defaultValue = field.DefaultValue;
            this.index = field.Index;
            this.selectList = string.Join(",", field.SelectList);
        }

        public ListFieldEditModel(CheckboxFieldInfo field)
        {
            this.id = field.ID;
            this.name = field.Name;
            this.required = field.Required;
            this.defaultValue = string.Join(",", field.DefaultValues);
            this.index = field.Index;
            this.selectList = string.Join(",", field.SelectList);
        }

        public int id;

        public string name;

        public bool required;

        public string selectList;

        public string defaultValue;

        public int index;
    }
}