using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core;
using Coldew.Api;

namespace LittleOrange.Core
{
    public class LittleOrangeObjectManager : ColdewObjectManager
    {
        LittleOrangeManager _crmManager;
        public LittleOrangeObjectManager(LittleOrangeManager crmManager)
            :base(crmManager)
        {
            this._crmManager = crmManager;
        }

        protected override ColdewObject Create(string id, string code, ColdewObjectType type, string name, bool isSystem)
        {
            if (code == LittleOrangeObjectConstCode.Object_GongsiKehu )
            {
                return new GongsiKehuColdewObject(id, code, name, this._crmManager);
            }
            else if (code == LittleOrangeObjectConstCode.Object_Ziranren)
            {
                return new ZiranrenColdewObject(id, code, name, this._crmManager);
            }
            return base.Create(id, code, type, name, isSystem);
        }
    }
}
