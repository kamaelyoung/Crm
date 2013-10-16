﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core;
using Coldew.Core.UI;

namespace Crm.Core
{
    public class ContactObject : ColdewObject
    {
        public ContactObject(string id, string code, string name, CrmManager crmManager)
            : base(id, code, name, crmManager)
        {

        }

        protected override FormManager CreateFormManager(ColdewManager coldewManager)
        {
            return base.CreateFormManager(coldewManager);
        }

        protected override MetadataManager CreateMetadataManager(ColdewManager coldewManager)
        {
            return new ContactManager(this, coldewManager.OrgManager);
        }

    }
}
