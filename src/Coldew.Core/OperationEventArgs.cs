using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core.Organization;

namespace Coldew.Core
{
    public class OperationEventArgs : EventArgs
    {
        public User Operator { set; get; }
    }
}
