using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core;
using Coldew.Core.UI;

namespace Douth.Core
{
    public class ContractObject : ColdewObject
    {
        public ContractObject(string id, string code, string name, DouthManager crmManager)
            : base(id, code, name, crmManager)
        {

        }

        protected override FormManager CreateFormManager(ColdewManager coldewManager)
        {
            return base.CreateFormManager(coldewManager);
        }

        protected override MetadataManager CreateMetadataManager(ColdewManager coldewManager)
        {
            return new ContractManager(this, coldewManager.OrgManager);
        }

        protected override GridViewManager CreateGridViewManager(ColdewManager coldewManager)
        {
            return new DouthGridViewManager(coldewManager.OrgManager, this);
        }

    }
}
