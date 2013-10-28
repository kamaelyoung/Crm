using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core;

namespace Douth.Core
{
    public class DouthObjectManager : ColdewObjectManager
    {
        DouthManager _crmManager;
        public DouthObjectManager(DouthManager crmManager)
            :base(crmManager)
        {
            this._crmManager = crmManager;
        }

        protected override ColdewObject Create(string id, string code, string name)
        {
            if (code == DouthObjectConstCode.FORM_Contract)
            {
                return new ContractObject(id, code, name, this._crmManager);
            }

            return base.Create(id, code, name);
        }
    }
}
