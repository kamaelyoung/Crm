using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core.Workflow;

namespace Coldew.Core.Workflow
{
    public class DemoLiuchengBiaodan
    {
        public DemoLiuchengBiaodan(Liucheng liucheng, int id, string neirong)
        {
            this.liucheng = liucheng;
            this.Neirong = neirong;
            this.Id = id;
        }

        public int Id { private set; get; }

        public string Neirong { private set; get; }

        public Liucheng liucheng { private set; get; }

        public void Xiugai(string neirong)
        {
            
            this.Neirong = neirong;
        }
    }
}
