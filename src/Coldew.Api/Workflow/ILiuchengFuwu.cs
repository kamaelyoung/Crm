using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Api.Workflow
{
    public interface ILiuchengFuwu
    {
        byte[] GetPngLiuchengtu(string liuchengId);

        List<LiuchengXinxi> GetLiuchengXinxiList(string liuchengMobanId, ShijianFanwei faqiShijianFanwei, ShijianFanwei jieshuShijianFanwei, string zhaiyao, int start, int size, out int count);

        void Chexiao(string[] liuchengId);
    }
}
