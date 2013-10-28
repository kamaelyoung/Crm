using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api.Organization;

namespace Coldew.Api.Workflow
{
    [Serializable]
    public class RenwuXinxi
    {
        public int Id { set; get; }

        public string Guid { set; get; }

        public string Bianhao { set; get; }

        public XingdongXinxi Xingdong { set; get; }

        public UserInfo Chuliren { set; get; }

        public RenwuZhuangtai Zhuangtai { set; get; }

        public DateTime? ChuliShijian { set; get; }

        public string ChuliShuoming { set; get; }
    }
}
