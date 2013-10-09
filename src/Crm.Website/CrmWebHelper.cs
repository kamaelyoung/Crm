using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using Crm.Api;
using System.Text;

namespace Crm.Website
{
    public class CrmWebHelper
    {
        static CrmWebHelper()
        {
            Spring.Context.IApplicationContext ctx = Spring.Context.Support.ContextRegistry.GetContext();
            CustomerAreaService = (ICustomerAreaService)ctx["CustomerAreaService"];

        }


        public static ICustomerAreaService CustomerAreaService { private set; get; }

        public static string CustomerAreaSelectOptions
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                List<CustomerAreaInfo> areaList = CustomerAreaService.GetAllArea();
                foreach (CustomerAreaInfo area in areaList)
                {
                    sb.AppendFormat("<option value='{0}'>{1}</option>", area.ID, area.Name);
                }

                return sb.ToString();
            }
        }
    }
}