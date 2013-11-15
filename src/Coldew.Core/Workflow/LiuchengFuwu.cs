using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api.Workflow;
using System.Drawing.Imaging;
using Coldew.Core.Organization;

namespace Coldew.Core.Workflow
{
    public class LiuchengFuwu : ILiuchengFuwu
    {
        LiuchengYinqing _yinqing;
        ColdewManager _coldewManger;

        public LiuchengFuwu(ColdewManager coldewManger)
        {
            this._yinqing = coldewManger.LiuchengYinqing;
            this._coldewManger = coldewManger;
        }

        public List<LiuchengXinxi> GetLiuchengXinxiList(string liuchengMobanId, ShijianFanwei faqiShijianFanwei, ShijianFanwei jieshuShijianFanwei, string zhaiyao, int start, int size, out int count)
        {
            List<Liucheng> liuchengList = this._yinqing.LiuchengManager.GetLiuchengList(liuchengMobanId, faqiShijianFanwei, jieshuShijianFanwei, zhaiyao, start, size, out count);
            return liuchengList.Select(x => x.Map()).ToList();
        }

        public LiuchengXinxi FaqiLiucheng(string liuchengMobanId, string code, string name, string shuoming, string faqirenAccount, bool jinjide, string zhaiyao, string biaodanId)
        {
            User user = this._coldewManger.OrgManager.UserManager.GetUserByAccount(faqirenAccount);
            LiuchengMoban moban = this._yinqing.LiuchengMobanManager.GetMobanById(liuchengMobanId);
            Metadata biaodan = moban.ColdewObject.MetadataManager.GetById(biaodanId);

            Liucheng liucheng = this._yinqing.LiuchengManager.FaqiLiucheng(user, liuchengMobanId, zhaiyao, jinjide, biaodan);
            Xingdong xingdong = liucheng.ChuangjianXingdong(code, name, zhaiyao, null);
            Renwu renwu = xingdong.ChuangjianRenwu(user);
            renwu.Wancheng(user, shuoming);
            xingdong.Wancheng();
            return liucheng.Map();
        }

        public LiuchengXinxi GetLiucheng(string liuchengId)
        {
            Liucheng liucheng = this._yinqing.LiuchengManager.GetLiucheng(liuchengId);
            return liucheng.Map();
        }

        public void Wancheng(string liuchengId)
        {
            Liucheng liucheng = this._yinqing.LiuchengManager.GetLiucheng(liuchengId);
            liucheng.Wancheng();
        }
    }
}
