using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Crm.Api;
using Crm.Website.Controllers;

namespace Crm.Website.Models
{
    public class CustomerAreaGridModel
    {
        public CustomerAreaGridModel(CustomerAreaInfo areaInfo, SetupController cntr)
        {
            this.id = areaInfo.ID;
            this.name = string.Format("<a href='{0}?areaId={1}'>{2}</a>", cntr.Url.Action("EditCustomerArea"), areaInfo.ID, areaInfo.Name);
            this.managers = string.Join(",", areaInfo.Managers.Select(x => x.Name));
        }

        public int id { set; get; }

        public string name { set; get; }

        public string managers { set; get; }
    }
}