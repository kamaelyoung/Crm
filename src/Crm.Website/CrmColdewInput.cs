using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Crm.Api;
using System.Text;
using Coldew.Website;
using Coldew.Api;
using Coldew.Api.Organization;

namespace Crm.Website
{
    public class CrmColdewInput : ColdewInput
    {
        bool _setDefaultValue;

        public CrmColdewInput(bool setDefaultValue)
            : base(setDefaultValue)
        {
            this._setDefaultValue = setDefaultValue;
        }

        public override MvcHtmlString Input(FieldInfo field)
        {
            switch (field.Type)
            {
                case CustomerFieldType.CustomerArea:
                    return CustomerArea((CustomerAreaFieldInfo)field);
            }

            return base.Input(field);
        }

        public MvcHtmlString CustomerArea(CustomerAreaFieldInfo field)
        {
            string dataRequiredAttr = "";
            if (field.Required)
            {
                dataRequiredAttr = "data-required = 'true'";
            }
            string template = @"<select class='input-large'  name='{0}' {1} >{2}</select>";
            StringBuilder itemSb = new StringBuilder();
            List<CustomerAreaInfo> areaList = CrmWebHelper.CustomerAreaService.GetAllArea();
            foreach (CustomerAreaInfo area in areaList)
            {
                itemSb.AppendFormat("<option value='{0}'>{1}</option>", area.ID, area.Name);
            }
            return new MvcHtmlString(string.Format(template, field.Code, dataRequiredAttr, itemSb.ToString()));
        }
    }
}