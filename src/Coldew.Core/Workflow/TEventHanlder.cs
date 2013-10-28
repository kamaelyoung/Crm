using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Core.Workflow
{
    public delegate void TEventHanlder<Sender, Args>(Sender sender, Args args);

    public delegate void TEventHanlder<Args>(Args args);
}
