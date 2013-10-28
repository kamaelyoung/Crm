using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Coldew.Core;
using Coldew.Core.Organization;

namespace Douth.Core
{
    public class DouthManager : ColdewManager
    {
        public DouthManager()
        {

        }

        protected override ColdewObjectManager CreateFormManager()
        {
            return new DouthObjectManager(this);
        }
    }
}
