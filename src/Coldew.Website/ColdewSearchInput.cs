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
    public class ColdewSearchInput
    {
        public virtual MvcHtmlString Input(FieldInfo field)
        {
            switch (field.Type)
            {
                case FieldType.DropdownList:
                case FieldType.RadioList:
                case FieldType.CheckboxList:
                    return DropdownList((ListFieldInfo)field);
                case FieldType.Number:
                    return Number((NumberFieldInfo)field);
                case FieldType.Date:
                case FieldType.ModifiedTime:
                case FieldType.CreatedTime:
                    return Date((DateFieldInfo)field);
                default:
                    return String(field);
            }
        }

        public virtual MvcHtmlString String(FieldInfo field)
        {
            return new MvcHtmlString(string.Format("<input class='keywordSearch' type='text' name='{0}' />", field.Code));
        }

        public virtual MvcHtmlString DropdownList(ListFieldInfo field)
        {
            string template = @"<select class='keywordSearch' name='{0}'>{1}</select>";
            StringBuilder itemSb= new StringBuilder();
            itemSb.Append("<option></option>");
            foreach (string item in field.SelectList)
            {
                itemSb.AppendFormat("<option>{0}</option>", item);
            }
            return new MvcHtmlString(string.Format(template, field.Code, itemSb.ToString()));
        }

        public virtual MvcHtmlString Number(NumberFieldInfo field)
        {
            return new MvcHtmlString(string.Format("<div class='numberSearch' data-field-code='{0}'><input type='text' name='min' class='input-small'/> <span>到</span><input type='text' name='max' class='input-small'/></div>", field.Code));
        }

        public virtual MvcHtmlString Date(DateFieldInfo field)
        {
            return new MvcHtmlString(string.Format("<div class='dateSearch' data-field-code='{0}'><input type='text' name='start' class='date input-small'/> <span>到</span><input type='text' name='end' class='date input-small'/></div>", field.Code));
        }
    }
}