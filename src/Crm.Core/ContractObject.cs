using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core;
using Coldew.Core.UI;
using Coldew.Core.DataServices;

namespace Crm.Core
{
    public class ContractObject : ColdewObject
    {
        public ContractObject(string id, string code, string name, CrmManager crmManager)
            : base(id, code, name, crmManager)
        {

        }

        protected override FormManager CreateFormManager(ColdewManager coldewManager)
        {
            return base.CreateFormManager(coldewManager);
        }

        protected override MetadataDataService CreateDataService()
        {
            return new ContractDataService(this);
        }
    }
}
