using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coldew.Api;
using System.Web.Mvc;

namespace Coldew.Website.Models
{
    public class FieldGridModel
    {
        public FieldGridModel(FieldInfo fieldInfo, Controller controller, string objectId)
        {
            this.id = fieldInfo.ID;
            this.name = fieldInfo.Name;
            this.index = fieldInfo.Index;
            this.required = fieldInfo.Required ? "是" : "否";
            this.type = fieldInfo.TypeName;
            switch (fieldInfo.Type)
            {
                case FieldType.CheckboxList:
                    this.editLink = string.Format("{0}?fieldId={2}&returnUrl={3}",
                        controller.Url.Action("EditCheckboxListField"),
                        fieldInfo.Code,
                        fieldInfo.ID,
                        controller.Url.Action("Extend", new { objectId = objectId }));
                    break;
                case FieldType.DropdownList:
                    this.editLink = string.Format("{0}?fieldId={2}&returnUrl={3}",
                        controller.Url.Action("EditDropdownListField"),
                        fieldInfo.Code,
                        fieldInfo.ID,
                        controller.Url.Action("Extend", new { objectId = objectId }));
                    break;
                case FieldType.RadioList:
                    this.editLink = string.Format("{0}?fieldId={2}&returnUrl={3}",
                        controller.Url.Action("EditRadioboxListField"),
                        fieldInfo.Code,
                        fieldInfo.ID,
                        controller.Url.Action("Extend", new { objectId = objectId }));
                    break;
                case FieldType.String:
                    this.editLink = string.Format("{0}?fieldId={2}&returnUrl={3}",
                        controller.Url.Action("EditStringField"),
                        fieldInfo.Code,
                        fieldInfo.ID,
                        controller.Url.Action("Extend", new { objectId = objectId }));
                    break;
                case FieldType.Text:
                    this.editLink = string.Format("{0}?fieldId={2}&returnUrl={3}",
                        controller.Url.Action("EditTextField"),
                        fieldInfo.Code,
                        fieldInfo.ID,
                        controller.Url.Action("Extend", new { objectId = objectId }));
                    break;
                case FieldType.Number:
                    this.editLink = string.Format("{0}?fieldId={2}&returnUrl={3}",
                        controller.Url.Action("EditNumberField"),
                        fieldInfo.Code,
                        fieldInfo.ID,
                        controller.Url.Action("Extend", new { objectId = objectId }));
                    break;
                case FieldType.Date:
                    this.editLink = string.Format("{0}?fieldId={2}&returnUrl={3}",
                        controller.Url.Action("EditDateField"),
                        fieldInfo.Code,
                        fieldInfo.ID,
                        controller.Url.Action("Extend", new { objectId = objectId }));
                    break;
                default: 
                    this.editLink = "#";
                    break;
            }
        }

        public int id { set; get; }

        public string name { set; get; }

        public string required { set; get; }

        public string type { set; get; }

        public int index { set; get; }

        public string editLink;
    }
}