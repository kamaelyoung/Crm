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
    public class ColdewInput
    {
        bool _setDefaultValue;

        public ColdewInput(bool setDefaultValue)
        {
            this._setDefaultValue = setDefaultValue;
        }

        public virtual MvcHtmlString Input(FieldInfo field)
        {
            switch (field.Type)
            {
                case FieldType.String:
                    return String((StringFieldInfo)field);
                case FieldType.Text:
                    return Text((StringFieldInfo)field);
                case FieldType.DropdownList:
                    return DropdownList((ListFieldInfo)field);
                case FieldType.RadioList:
                    return RadioList((ListFieldInfo)field);
                case FieldType.CheckboxList:
                    return CheckboxList((CheckboxFieldInfo)field);
                case FieldType.Number:
                    return Number((NumberFieldInfo)field);
                case FieldType.Date:
                    return Date((DateFieldInfo)field);
                case FieldType.User:
                    return User((UserFieldInfo)field);
                case FieldType.UserList:
                    return UserList((UserListFieldInfo)field);
                case FieldType.Metadata:
                    return Metadata((MetadataFieldInfo)field);
            }

            throw new ArgumentException("field.Type:" + field.Type.ToString());
        }

        public MvcHtmlString String(StringFieldInfo field)
        {
            string dataRequiredAttr = "";
            if (field.Required) { 
                dataRequiredAttr = "data-required = 'true'";
            }
            string defualtValue = "";
            if(this._setDefaultValue)
            {
                defualtValue = field.DefaultValue;
            }
            return new MvcHtmlString(string.Format("<input type='text' class='input-large' name='{0}' {1} value='{2}'/>", field.Code, dataRequiredAttr, defualtValue));
        }

        public MvcHtmlString Text(StringFieldInfo field)
        {
            string dataRequiredAttr = "";
            if (field.Required)
            {
                dataRequiredAttr = "data-required = 'true'";
            }
            string defualtValue = "";
            if (this._setDefaultValue)
            {
                defualtValue = field.DefaultValue;
            }

            return new MvcHtmlString(string.Format("<textarea class='input-large' name='{0}' {1} rows='3' >{2}</textarea>", field.Code, dataRequiredAttr, defualtValue));
        }

        public MvcHtmlString DropdownList(ListFieldInfo field)
        {
            string dataRequiredAttr = "";
            if (field.Required)
            {
                dataRequiredAttr = "data-required = 'true'";
            }
            string template = @"<select class='input-large'  name='{0}' {1} >{2}</select>";
            StringBuilder itemSb= new StringBuilder();
            itemSb.Append("<option></option>");
            foreach (string item in field.SelectList)
            {
                if (this._setDefaultValue && item == field.DefaultValue)
                {
                    itemSb.AppendFormat("<option selected='selected'>{0}</option>", item);
                }
                else
                {
                    itemSb.AppendFormat("<option>{0}</option>", item);
                }
            }
            return new MvcHtmlString(string.Format(template, field.Code, dataRequiredAttr, itemSb.ToString()));
        }

        public MvcHtmlString RadioList(ListFieldInfo field)
        {
            string dataRequiredAttr = "";
            if (field.Required)
            {
                dataRequiredAttr = "data-required = 'true'";
            }

            StringBuilder sb = new StringBuilder();
            foreach (string item in field.SelectList)
            {
                sb.Append("<label class='radio'>");
                if (this._setDefaultValue && item == field.DefaultValue)
                {
                    sb.AppendFormat("<input type='radio' name='{0}' {1} checked='checked' value='{2}'/>", field.Code, dataRequiredAttr, item);
                }
                else
                {
                    sb.AppendFormat("<input type='radio' name='{0}' {1} value='{2}'/>", field.Code, dataRequiredAttr, item);
                }
                sb.Append("</label>");
            }
            return new MvcHtmlString(sb.ToString());
        }

        public MvcHtmlString CheckboxList(CheckboxFieldInfo field)
        {
            string dataRequiredAttr = "";
            if (field.Required)
            {
                dataRequiredAttr = "data-required = 'true'";
            }

            StringBuilder sb = new StringBuilder();
            foreach (string item in field.SelectList)
            {
                sb.Append("<label class='checkbox'>");
                if (this._setDefaultValue && field.DefaultValues.Contains(item))
                {
                    sb.AppendFormat("<input type='checkbox' name='{0}' {1} checked='checked' value='{2}'/>", field.Code, dataRequiredAttr, item);
                }
                else
                {
                    sb.AppendFormat("<input type='checkbox' name='{0}' {1} value='{2}'/>", field.Code, dataRequiredAttr, item);
                }
                sb.Append("</label>");
            }
            return new MvcHtmlString(sb.ToString());
        }

        public MvcHtmlString Number(NumberFieldInfo field)
        {
            string dataRequiredAttr = "";
            if (field.Required)
            {
                dataRequiredAttr = "data-required = 'true'";
            }
            string defualtValue = "";
            if (this._setDefaultValue && field.DefaultValue.HasValue)
            {
                defualtValue = field.DefaultValue.ToString();
            }

            return new MvcHtmlString(string.Format("<input type='text' class='input-large' name='{0}' {1} value='{2}'/>", field.Code, dataRequiredAttr, defualtValue));
        }

        public MvcHtmlString Date(DateFieldInfo field)
        {
            string dataRequiredAttr = "";
            if (field.Required)
            {
                dataRequiredAttr = "data-required = 'true'";
            }
            string defualtValue = "";
            if (this._setDefaultValue)
            {
                defualtValue = field.DefaultValue;
            }

            return new MvcHtmlString(string.Format("<input type='text' class='input-large date' name='{0}' {1} value='{2}'/>", field.Code, dataRequiredAttr, defualtValue));
        }

        public MvcHtmlString User(UserFieldInfo field)
        {
            string dataRequiredAttr = "";
            if (field.Required)
            {
                dataRequiredAttr = "data-required = 'true'";
            }

            IList<UserInfo> users = WebHelper.UserService.GetAllNormalUser().ToList();
            StringBuilder sb = new StringBuilder();
            foreach (UserInfo user in users)
            {
                if (user.Account == WebHelper.CurrentUserAccount)
                {
                    sb.AppendFormat("<label class='checkbox'><input type='checkbox' name='{0}' checked='checked' {3} value='{1}' />{2}</label>", field.Code, user.Account, user.Name, dataRequiredAttr);
                }
                else
                {
                    sb.AppendFormat("<label class='checkbox'><input type='checkbox' name='{0}' {3} value='{1}' />{2}</label>", field.Code, user.Account, user.Name, dataRequiredAttr);
                }
            }

            return new MvcHtmlString(sb.ToString());
        }

        public MvcHtmlString UserList(UserListFieldInfo field)
        {
            string dataRequiredAttr = "";
            if (field.Required)
            {
                dataRequiredAttr = "data-required = 'true'";
            }

            IList<UserInfo> users = WebHelper.UserService.GetAllNormalUser().ToList();
            StringBuilder sb = new StringBuilder();
            foreach (UserInfo user in users)
            {
                if (user.Account == WebHelper.CurrentUserAccount)
                {
                    sb.AppendFormat("<label class='checkbox'><input type='checkbox' name='{0}' checked='checked' {3} value='{1}' />{2}</label>", field.Code, user.Account, user.Name, dataRequiredAttr);
                }
                else
                {
                    sb.AppendFormat("<label class='checkbox'><input type='checkbox' name='{0}' {3} value='{1}' />{2}</label>", field.Code, user.Account, user.Name, dataRequiredAttr);
                }
            }

            return new MvcHtmlString(sb.ToString());
        }

        public MvcHtmlString Metadata(MetadataFieldInfo field)
        {
            string template = @"<div class='metadataSelect' data-form-id='{0}' data-form-name='{1}'> 
            <input type='text' readonly='readonly' class='input-large txtName'/>
            <input class='txtId' type='hidden' name='{2}'/>
            <button class='btn btnSelect'>选择</button> </div>";
            return new MvcHtmlString(string.Format(template, field.ValueFormId, field.ValueFormName, field.Code));
        }
    }
}