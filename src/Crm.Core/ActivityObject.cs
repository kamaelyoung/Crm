using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core;
using Coldew.Core.UI;
using Coldew.Core.DataServices;

namespace Crm.Core
{
    public class ActivityObject : ColdewObject
    {
        public ActivityObject(string id, string code, string name, CrmManager crmManager)
            : base(id, code, name, crmManager)
        {

        }

        protected override MetadataDataService CreateDataService()
        {
            return new ActivityDataService(this);
        }

    }
}
