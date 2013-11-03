using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api.Workflow;
using Coldew.Core.Organization;

namespace Coldew.Core.Workflow
{
    public class YinqingFuwu : IYinqingFuwu
    {
        LiuchengYinqing _yingqing;

        public YinqingFuwu(ColdewManager coldewManger)
        {
            this._yingqing = coldewManger.LiuchengYinqing;
        }

        public List<LiuchengMobanXinxi> GetLiuchengMobanByYonghu(string yonghuZhanghao)
        {
            User yonghu = this._yingqing.GetYonghu(yonghuZhanghao);
            return this._yingqing.LiuchengMobanManager.GetAllMoban().Where(x => x.NengFaqi(yonghu)).Select(x => x.Map()).ToList();
        }

        public List<LiuchengMobanXinxi> GetSuoyouLiuchengMoban()
        {
            return this._yingqing.LiuchengMobanManager.GetAllMoban().Select(x => x.Map()).ToList();
        }
    }
}
