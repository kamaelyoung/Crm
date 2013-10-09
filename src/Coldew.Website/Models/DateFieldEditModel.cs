using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coldew.Api;

namespace Coldew.Website.Models
{
    public class DateFieldEditModel
    {
        public DateFieldEditModel(DateFieldInfo field)
        {
            this.id = field.ID;
            this.name = field.Name;
            this.required = field.Required;
            this.defaultValueIsToday = field.DefaultValueIsToday;
            this.index = field.Index;
        }

        public int id;

        public string name;

        public bool required;

        public bool defaultValueIsToday;

        public int index;
    }
}