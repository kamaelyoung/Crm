using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core;
using Crm.Api;

namespace Crm.Core
{
    public class CrmObjectManager : ColdewObjectManager
    {
        CrmManager _crmManager;
        public CrmObjectManager(CrmManager crmManager)
            :base(crmManager)
        {
            this._crmManager = crmManager;
        }

        protected override ColdewObject Create(string id, string code, string name)
        {
            if (code == CrmObjectConstCode.FORM_CUSTOMER)
            {
                return new CustomerObject(id, code, name, this._crmManager);
            }
            else if (code == CrmObjectConstCode.FORM_CONTACT)
            {
                return new ContactObject(id, code, name, this._crmManager);
            }
            else if (code == CrmObjectConstCode.FORM_Activity)
            {
                return new ActivityObject(id, code, name, this._crmManager);
            }
            else if (code == CrmObjectConstCode.FORM_Contract)
            {
                return new ContractObject(id, code, name, this._crmManager);
            }

            return base.Create(id, code, name);
        }
    }
}
