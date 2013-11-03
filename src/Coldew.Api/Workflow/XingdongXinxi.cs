using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Api.Workflow
{
    [Serializable]
    public class XingdongXinxi
    {
        public int Id { set; get; }

        public string Guid { set; get; }

        public LiuchengXinxi liucheng { set; get; }

        public string LiuchengMingcheng { set; get; }

        public string Code { set; get; }

        public string Name { set; get; }

        public bool Jinjide { set; get; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime KaishiShijian { set; get; }

        /// <summary>
        /// 期望完成时间
        /// </summary>
        public DateTime? QiwangWanchengShijian { set; get; }

        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime? WanchengShijian { set; get; }

        public string Zhaiyao { set; get; }

        public XingdongZhuangtai Zhuangtai { set; get; }
    }
}
