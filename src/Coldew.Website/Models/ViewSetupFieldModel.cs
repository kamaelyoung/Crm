using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coldew.Api;

namespace Coldew.Website.Models
{
    public class ViewSetupFieldModel
    {
        public ViewSetupFieldModel(FieldInfo field, bool selected, int width)
        {
            this.fieldId = field.ID;
            this.name = field.Name;
            this.required = field.Required;
            this.selected = selected;
            this.width = width;
            this.index = field.Index;
        }

        public int fieldId;

        public string name;

        public bool required;

        public bool selected;

        public string checkedAttr 
        { 
            get 
            { 
                if (selected) {
                    return "checked='checked'";
                } 
                return "";
            } 
        }

        public int width;

        public int index;
    }
}