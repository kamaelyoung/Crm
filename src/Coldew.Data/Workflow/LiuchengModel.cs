using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Data
{
    public class LiuchengModel
    {
        public virtual int Id { set; get; }

        public virtual string Guid { set; get; }

        public virtual string MobanId { set; get; }

        public virtual string Mingcheng { set; get; }

        public virtual string Faqiren { set; get; }

        public virtual DateTime FaqiShijian { set; get; }

        public virtual DateTime? JieshuShijian { set; get; }

        public virtual int Zhuangtai { set; get; }

        public virtual bool Jinjide { set; get; }

        public virtual string BiaodanId { set; get; }

        public virtual string Zhaiyao { set; get; }
    }
}
