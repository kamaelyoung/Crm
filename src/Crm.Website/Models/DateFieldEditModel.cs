using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Crm.Api;

namespace Crm.Website.Models
{
    public class DateFieldEditModel
    {
        public DateFieldEditModel(FieldInfo field, DateFieldConfigInfo configInfo)
        {
            this.id = field.ID;
            this.name = field.Name;
            this.required = field.Required;
            this.defaultValueIsToday = configInfo.DefaultValueIsToday;
            this.index = field.Index;
        }

        public int id;

        public string name;

        public bool required;

        public bool defaultValueIsToday;

        public int index;
    }
}