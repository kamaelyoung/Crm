using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Data;
using Crm.Api;
using System.Text.RegularExpressions;
using Crm.Api.Exceptions;
using Newtonsoft.Json;
using Coldew.Core;
using Coldew.Core.Organization;
using Coldew.Core.DataServices;

namespace Crm.Core
{
    public class CustomerManager : MetadataManager
    {

        public CustomerManager(ColdewObject cobject, MetadataDataService dataService, OrganizationManagement orgManger)
            : base(cobject, dataService, orgManger)
        {
            
        }

        public int GetAreaCustomerCount(CustomerArea args)
        {
            int count = this._metadataList.Where(x => ((Customer)x).Area == args).Count();
            return count;
        }
    }
}
