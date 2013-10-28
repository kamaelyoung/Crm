using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core;
using Coldew.Core.Workflow;
using Coldew.Core.Organization;

namespace Coldew.Core.Workflow
{
    public class DemoLiuchengYinqing : Yinqing
    {
        public DemoLiuchengYinqing(ColdewManager coldewManager)
            : base(coldewManager)
        {

        }

        protected override List<LiuchengMoban> JiazaiLiuchengMoban()
        {
            List<LiuchengMoban> liuchengList = new List<LiuchengMoban>();
            DemoLiuchengMoban = new DemoLiuchengMoban(1, "DC74D579-43EC-4686-9B8D-83FFD3516E99", "测试流程", "http://localhost:8942/DemoLiucheng/Faqi",
                "http://localhost:8942/DemoLiucheng/XingdongChuli", "http://localhost:8942/DemoLiucheng/LiuchengXinxi", "");
            liuchengList.Add(DemoLiuchengMoban);

            DemoLiuchengMoban1 = new DemoLiuchengMoban(2, "DC74D579-43EC-4686-9B8D-83FFD3516E95", "测试流程1", "http://localhost:8942/DemoLiucheng/Faqi",
                "http://localhost:8942/DemoLiucheng/XingdongChuli", "http://localhost:8942/DemoLiucheng/LiuchengXinxi", "");
            liuchengList.Add(DemoLiuchengMoban1);
            return liuchengList;
        }

        public DemoLiuchengMoban DemoLiuchengMoban { private set; get; }

        public DemoLiuchengMoban DemoLiuchengMoban1 { private set; get; }
    }
}
