using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Crm.Api;

namespace Crm.Website.Models
{
    public class StringFieldEditModel
    {
        public StringFieldEditModel(StringFieldInfo field)
        {
            this.id = field.ID;
            this.name = field.Name;
            this.required = field.Required;
            this.defaultValue = field.DefaultValue;
            this.index = field.Index;
        }

        public int id;

        public string name;

        public bool required;

        public string defaultValue;

        public int index;
    }
}