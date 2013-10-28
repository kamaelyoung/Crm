using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Api.Workflow
{
    [Serializable]
    public class LiuchengMobanXinxi
    {
        public int Id { set; get; }

        public string Guid { set; get; }

        public string Mingcheng { set; get; }

        public string FaqiUrl { set; get; }

        public string RenwuUrl { set; get; }

        public string GuidangUrl { set; get; }

        public string Shuoming { set; get; }
    }
}
