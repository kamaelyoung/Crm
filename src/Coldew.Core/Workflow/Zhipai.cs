using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core.Organization;

namespace Coldew.Core.Workflow
{
    public class Zhipai
    {
        public Zhipai(User zhipairen, User dailiren, Renwu renwu)
        {
            this.Renwu = renwu;
            this.Zhipairen = zhipairen;
            this.Dailiren = dailiren;
        }

        public Renwu Renwu { private set; get; }

        public User Zhipairen { private set; get; }

        public User Dailiren { private set; get; }
    }
}
