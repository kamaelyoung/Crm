using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core;

namespace Crm.Core
{
    public class CrmFormManager : FormManager
    {
        CrmManager _crmManager;
        public CrmFormManager(CrmManager crmManager)
            :base(crmManager)
        {
            this._crmManager = crmManager;
        }

        protected override Form Create(string id, string code, string name)
        {
            return new CrmForm(id, code, name, this._crmManager);
        }
    }
}
