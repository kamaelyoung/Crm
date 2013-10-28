using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Data
{
    public class JianglaiRenwuZhipaiModel
    {
        public virtual int Id { set; get; }

        public virtual string Zhipairen { set; get; }

        public virtual string Dailiren { set; get; }

        public virtual DateTime? KaishiShijian { set; get; }

        public virtual DateTime? JieshuShijian { set; get; }
    }
}
