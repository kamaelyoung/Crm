using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldew.Api.Workflow
{
    public interface IYinqingFuwu
    {
        List<LiuchengMobanXinxi> GetLiuchengMobanByYonghu(string yonghu);
        List<LiuchengMobanXinxi> GetSuoyouLiuchengMoban();
    }
}
