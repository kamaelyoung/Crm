using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Data;
using log4net;
using Crm.Api;
using Coldew.Core;
using Coldew.Core.Organization;

namespace Crm.Core
{
    public class CrmManager : ColdewManager
    {
        public CrmManager()
        {
        }

        protected override void Init()
        {
            base.Init();
            this.AreaManager = new CustomerAreaManager(this.OrgManager, this.ObjectManager);
        }

        protected override void Load()
        {
            this.AreaManager.Load();
            base.Load();
        }

        public CustomerAreaManager AreaManager { set; get; }

        protected override ColdewObjectManager CreateFormManager()
        {
            return new CrmObjectManager(this);
        }
    }
}
