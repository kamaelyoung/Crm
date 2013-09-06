using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Crm.Api;

namespace Crm.Website.Models
{
    public class CustomerAreaEditModel
    {
        public CustomerAreaEditModel()
        {

        }

        public CustomerAreaEditModel(CustomerAreaInfo areaInfo)
        {
            this.id = areaInfo.ID;
            this.name = areaInfo.Name;
            this.managerAccounts = areaInfo.Managers.Select(x => x.Account).ToList();
        }

        public int id;
        public string name;
        public List<string> managerAccounts;
    }
}