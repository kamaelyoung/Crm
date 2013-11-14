using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coldew.Api;

namespace Coldew.Website.Models
{
    public class StringFieldEditModel
    {
        public StringFieldEditModel(StringFieldInfo field)
        {
            this.id = field.ID;
            this.name = field.Name;
            this.required = field.Required;
            this.defaultValue = field.DefaultValue;
            
        }

        public int id;

        public string name;

        public bool required;

        public string defaultValue;

        public int index;
    }
}