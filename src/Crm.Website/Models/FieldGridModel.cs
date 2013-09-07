using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Crm.Api;
using System.Web.Mvc;

namespace Crm.Website.Models
{
    public class FieldGridModel
    {
        public FieldGridModel(FieldInfo fieldInfo, Controller controller)
        {
            this.id = fieldInfo.ID;
            this.name = fieldInfo.Name;
            this.index = fieldInfo.Index;
            this.required = fieldInfo.Required ? "是" : "否";
            this.type = this.Map(fieldInfo.Type);
            switch (fieldInfo.Type)
            {
                case FieldType.CheckboxList:
                    this.editLink = string.Format("{0}?formType={1}&fieldId={2}&returnUrl={3}",
                        controller.Url.Action("EditCheckboxListField"),
                        fieldInfo.FormType,
                        fieldInfo.ID,
                        controller.Url.Action("Extend", new { formType = fieldInfo.FormType }));
                    break;
                case FieldType.DropdownList:
                    this.editLink = string.Format("{0}?formType={1}&fieldId={2}&returnUrl={3}",
                        controller.Url.Action("EditDropdownListField"),
                        fieldInfo.FormType,
                        fieldInfo.ID,
                        controller.Url.Action("Extend", new { formType = fieldInfo.FormType }));
                    break;
                case FieldType.RadioList:
                    this.editLink = string.Format("{0}?formType={1}&fieldId={2}&returnUrl={3}",
                        controller.Url.Action("EditRadioboxListField"),
                        fieldInfo.FormType,
                        fieldInfo.ID,
                        controller.Url.Action("Extend", new { formType = fieldInfo.FormType }));
                    break;
                case FieldType.String:
                    this.editLink = string.Format("{0}?formType={1}&fieldId={2}&returnUrl={3}",
                        controller.Url.Action("EditStringField"),
                        fieldInfo.FormType,
                        fieldInfo.ID,
                        controller.Url.Action("Extend", new { formType = fieldInfo.FormType }));
                    break;
                case FieldType.Text:
                    this.editLink = string.Format("{0}?formType={1}&fieldId={2}&returnUrl={3}",
                        controller.Url.Action("EditTextField"),
                        fieldInfo.FormType,
                        fieldInfo.ID,
                        controller.Url.Action("Extend", new { formType = fieldInfo.FormType }));
                    break;
                case FieldType.Number:
                    this.editLink = string.Format("{0}?formType={1}&fieldId={2}&returnUrl={3}",
                        controller.Url.Action("EditNumberField"),
                        fieldInfo.FormType,
                        fieldInfo.ID,
                        controller.Url.Action("Extend", new { formType = fieldInfo.FormType }));
                    break;
                case FieldType.Date:
                    this.editLink = string.Format("{0}?formType={1}&fieldId={2}&returnUrl={3}",
                        controller.Url.Action("EditDateField"),
                        fieldInfo.FormType,
                        fieldInfo.ID,
                        controller.Url.Action("Extend", new { formType = fieldInfo.FormType }));
                    break;
                default:
                    throw new ArgumentException("fieldInfo.Type");
            }
        }

        private string Map(FieldType type)
        {
            switch (type)
            {
                case FieldType.CheckboxList:
                    return "复选框";
                case FieldType.DropdownList:
                    return "下拉选项";
                case FieldType.RadioList:
                    return "单选框";
                case FieldType.String:
                    return "短文本";
                case FieldType.Text:
                    return "长文本";
                case FieldType.Date:
                    return "日期";
                case FieldType.Number:
                    return "数字";
                    
            }
            return "";
        }

        public int id { set; get; }

        public string name { set; get; }

        public string required { set; get; }

        public string type { set; get; }

        public int index { set; get; }

        public string editLink;
    }
}