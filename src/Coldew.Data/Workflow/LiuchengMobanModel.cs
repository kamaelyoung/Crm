using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Data
{
    public class LiuchengMobanModel
    {
        public virtual string ID { set; get; }

        public virtual string Code { set; get; }

        public virtual string Name { set; get; }

        public virtual string ObjectCode { set; get; }

        public virtual string FaqiFormCode { set; get; }

        public virtual string BuzhouListJson { set; get; }

        public virtual string Remark { set; get; }
    }
}
