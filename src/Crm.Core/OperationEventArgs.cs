using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Core.Organization;

namespace Crm.Core
{
    public class OperationEventArgs : EventArgs
    {
        public User Operator { set; get; }
    }
}
