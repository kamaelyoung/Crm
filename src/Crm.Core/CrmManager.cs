using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Data;
using log4net;
using Crm.Api;
using Coldew.Core;

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
            this.AreaManager = new CustomerAreaManager(this.OrgManager, this.FormManager);
        }

        protected override void Load()
        {
            this.AreaManager.Load();
            base.Load();
        }

        public CustomerAreaManager AreaManager { set; get; }

        protected override FormManager CreateFormManager()
        {
            return new CrmFormManager(this);
        }
    }
}
