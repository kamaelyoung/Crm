using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Coldew.Core;
using Coldew.Core.Organization;

namespace LittleOrange.Core
{
    public class LittleOrangeManager : ColdewManager
    {
        public LittleOrangeManager()
        {

        }

        protected override ColdewObjectManager CreateFormManager()
        {
            return new LittleOrangeObjectManager(this);
        }
    }
}
