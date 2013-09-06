using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Crm.Api;

namespace Crm.Website.Models
{
    public class ViewSetupFieldModel
    {
        public ViewSetupFieldModel()
        {
            this.width = 80;
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
    }
}