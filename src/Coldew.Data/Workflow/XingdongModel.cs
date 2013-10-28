using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Data
{
    public class XingdongModel
    {
        public virtual int Id { set ; get; }

        public virtual string Guid { set; get; }

        public virtual int LiuchengId { set; get; }

        public virtual string Bianhao { set ; get; }

        public virtual string Mingcheng { set; get; }

        public virtual bool Jinjide { set; get; } 

        /// <summary>
        /// 开始时间
        /// </summary>
        public virtual DateTime KaishiShijian { set ; get; }

        /// <summary>
        /// 期望完成时间
        /// </summary>
        public virtual DateTime? QiwangWanchengShijian { set ; get; }

        /// <summary>
        /// 实际完成时间
        /// </summary>
        public virtual DateTime? WanchengShijian { set ; get; }

        public virtual int Zhuangtai { set; get; }

        public virtual int? WanchengJieguo { set; get; }

        public virtual bool Tuihuide { set; get; }

        public virtual int Leixing { set; get; }

        public virtual string Zhaiyao { set; get; }
    }
}
