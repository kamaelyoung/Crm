using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Crm.Api;
using System.Text;
using Coldew.Website;
using Coldew.Api;

namespace Crm.Website
{
    public class CrmColdewSearchInput : ColdewSearchInput
    {
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
            string template = @"<select class='keywordSearch' name='{0}'>{1}</select>";
            StringBuilder itemSb = new StringBuilder();
            itemSb.Append("<option></option>");
            List<CustomerAreaInfo> areaList = CrmWebHelper.CustomerAreaService.GetAllArea();
            foreach (CustomerAreaInfo area in areaList)
            {
                itemSb.AppendFormat("<option>{0}</option>", area.Name);
            }
            return new MvcHtmlString(string.Format(template, field.Code, itemSb.ToString()));
        }
    }
}