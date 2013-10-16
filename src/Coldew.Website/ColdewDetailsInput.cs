using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Coldew.Api;

namespace Coldew.Website
{
    public class ColdewDetailsInput
    {
        public virtual MvcHtmlString Input(FieldInfo field, Dictionary<string, PropertyInfo> metadataPropertys)
        {
            string value = "";
            if (metadataPropertys.ContainsKey(field.Code))
            {
                PropertyInfo propertyInfo = metadataPropertys[field.Code];
                value = propertyInfo.ShowValue;
            }
            return new MvcHtmlString(string.Format("<label style='padding-top: 5px;'>{0}</label>", value));
        }
    }
}