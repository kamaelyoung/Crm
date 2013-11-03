using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core;
using Coldew.Core.UI;
using Coldew.Data;

namespace LittleOrange.Core
{
    public class ZhiranrenObject : ColdewObject
    {
        LittleOrangeManager _crmManager;
        public ZhiranrenObject(string id, string code, string name, LittleOrangeManager crmManager)
            :base(id, code, name, crmManager)
        {
            this._crmManager = crmManager;
        }

        protected override FormManager CreateFormManager(ColdewManager coldewManager)
        {
            return base.CreateFormManager(coldewManager);
        }

        protected override MetadataManager CreateMetadataManager(ColdewManager coldewManager)
        {
            return new ZiranrenManager(this, coldewManager.OrgManager);
        }

    }
}
