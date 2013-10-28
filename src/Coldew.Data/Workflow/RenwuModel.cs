using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Data
{
    public class RenwuModel
    {
        public virtual int Id { set; get; }

        public virtual string Guid { set; get; }

        public virtual string Bianhao { set; get; }

        public virtual int XingdongId { set; get; }

        public virtual string Yongyouren { set; get; }

        public virtual string Chuliren { set; get; }

        public virtual int Zhuangtai { set; get; }

        public virtual string ShijiChuliren { set; get; }

        public virtual DateTime? ChuliShijian { set; get; }

        public virtual int? ChuliJieguo { set; get; }

        public virtual string ChuliShuoming { set; get; }
    }
}
