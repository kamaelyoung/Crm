﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core;

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

        protected override ColdewObject Create(string id, string code, string name)
        {
            if (code == LittleOrangeObjectConstCode.Object_GongsiKehu ||
                code == LittleOrangeObjectConstCode.Object_Ziranren)
            {
                return new CustomerObject(id, code, name, this._crmManager);
            }

            return base.Create(id, code, name);
        }
    }
}
