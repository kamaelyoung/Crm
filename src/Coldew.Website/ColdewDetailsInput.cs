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
        private const string ControlGroupTemplate = @"<div class='control-group'>
                    <label class='control-label' >{0}：</label>
                    <div class='controls'>
                        {1}
                    </div>
                </div>";

        public virtual MvcHtmlString Input(FieldInfo field, Dictionary<string, PropertyInfo> metadataPropertys)
        {
            string value = "";
            if (metadataPropertys.ContainsKey(field.Code))
            {
                PropertyInfo propertyInfo = metadataPropertys[field.Code];
                value = propertyInfo.ShowValue;
            }
            return ControlGroup(field, string.Format("<label style='padding-top: 5px;'>{0}</label>", value));
        }

        private MvcHtmlString ControlGroup(FieldInfo field, string inputHtml)
        {
            string html = string.Format(ControlGroupTemplate, field.Name, inputHtml);
            return new MvcHtmlString(html);
        }
    }
}