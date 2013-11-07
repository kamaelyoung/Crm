using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Api.Workflow
{
    public interface ILiuchengFuwu
    {
        List<LiuchengXinxi> GetLiuchengXinxiList(string liuchengMobanId, ShijianFanwei faqiShijianFanwei, ShijianFanwei jieshuShijianFanwei, string zhaiyao, int start, int size, out int count);
        LiuchengXinxi FaqiLiucheng(string liuchengMobanId, string code, string name, string shuoming, string faqirenAccount, bool jinjide, string zhaiyao, string biaodanId);
        LiuchengXinxi GetLiucheng(string liuchengId);
        void Wancheng(string liuchengId);
    }
}
