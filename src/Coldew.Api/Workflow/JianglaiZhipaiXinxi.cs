using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api.Organization;

namespace Coldew.Api.Workflow
{
    [Serializable]
    public class JianglaiZhipaiXinxi
    {
        public UserInfo Dailiren { set; get; }

        public DateTime? KaishiShijian { set; get; }

        public DateTime? JieshuShijian { set; get; }
    }
}
