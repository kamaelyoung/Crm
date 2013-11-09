using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Coldew.Api;
using System.Text;
using Coldew.Api.Organization;

namespace Coldew.Website
{
    public class ColdewGridViewFilterInput : ColdewSearchInput
    {

        public override MvcHtmlString Date(DateFieldInfo field)
        {
            return new MvcHtmlString(string.Format("<div class='dateSearch' data-field-code='{0}'>最近<input type='text' name='start' class='input-mini'/> <span>天到</span><input type='text' name='end' class='input-mini'/>天</div>", field.Code));
        }
    }
}