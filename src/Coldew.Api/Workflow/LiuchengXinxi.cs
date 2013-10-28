using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api.Organization;

namespace Coldew.Api.Workflow
{
    [Serializable]
    public class LiuchengXinxi
    {
        public int Id { set; get; }

        public string Guid { set; get; }

        public LiuchengMobanXinxi Liucheng { set; get; }

        public string Mingcheng { set; get; }

        public UserInfo Faqiren { set; get; }

        public DateTime FaqiShijian { set; get; }

        public DateTime? JieshuShijian { set; get; }

        public LiuchengZhuangtai Zhuangtai { set; get; }

        public string Zhaiyao { set; get; }
    }
}
