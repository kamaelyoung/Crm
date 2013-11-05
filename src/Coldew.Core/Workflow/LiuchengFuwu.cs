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
            List<Liucheng> liuchengList = new List<Liucheng>();
            foreach (LiuchengMoban moban in this._yinqing.LiuchengMobanManager.GetAllMoban())
            {
                if (!string.IsNullOrEmpty(liuchengMobanId) && moban.ID != liuchengMobanId)
                {
                    continue;
                }
                foreach (Liucheng liucheng in moban.LiuchengList)
                {
                    if (faqiShijianFanwei != null && !faqiShijianFanwei.ZaiFanweinei(liucheng.FaqiShijian))
                    {
                        continue;
                    }
                    if (jieshuShijianFanwei != null && !jieshuShijianFanwei.ZaiFanweinei(liucheng.JieshuShijian))
                    {
                        continue;
                    }
                    if (zhaiyao != null && liucheng.Zhaiyao.IndexOf(zhaiyao, StringComparison.InvariantCultureIgnoreCase) == -1)
                    {
                        continue;
                    }
                    liuchengList.Add(liucheng);
                }
            }
            count = liuchengList.Count;
            return liuchengList.Select(x => x.Map()).ToList();
        }

        public LiuchengXinxi FaqiLiucheng(string liuchengMobanId, string faqirenAccount, bool jinjide, string zhaiyao, string biaodanId)
        {
            User user = this._coldewManger.OrgManager.UserManager.GetUserByAccount(faqirenAccount);
            LiuchengMoban moban = this._yinqing.LiuchengMobanManager.GetMobanById(liuchengMobanId);
            Metadata biaodan = moban.BiandanManager.GetById(biaodanId);

            Liucheng liucheng = moban.FaqiLiucheng(user, zhaiyao, jinjide, biaodan);
            return liucheng.Map();
        }

        public LiuchengXinxi GetLiucheng(string liuchengId)
        {
            Liucheng liucheng = this._yinqing.LiuchengMobanManager.GetLiucheng(liuchengId);
            return liucheng.Map();
        }

        public void Wancheng(string liuchengId)
        {
            Liucheng liucheng = this._yinqing.LiuchengMobanManager.GetLiucheng(liuchengId);
            liucheng.Wancheng();
        }
    }
}
