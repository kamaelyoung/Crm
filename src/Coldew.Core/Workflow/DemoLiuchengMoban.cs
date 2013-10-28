using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing.Imaging;
using Coldew.Core.Workflow;
using Coldew.Api.Workflow;
using Coldew.Core.Organization;

namespace Coldew.Core.Workflow
{
    public class DemoLiuchengMoban : LiuchengMoban
    {
        public DemoLiuchengMoban(int id, string guid, string mingcheng, string faqiUrl, string renwuUrl, string guidangUrl, string shuoming)
            : base(id, guid, mingcheng, faqiUrl, renwuUrl, guidangUrl, shuoming)
        {
            _biaodanList = new List<DemoLiuchengBiaodan>();
        }

        List<DemoLiuchengBiaodan> _biaodanList;
        public List<DemoLiuchengBiaodan> BiaodanList
        {
            get
            {
                return this._biaodanList.ToList();
            }
        }

        public Liucheng Faqi(User faqiren, string neirong, bool jinjide)
        {
            Liucheng liucheng = this.FaqiLiucheng(faqiren, neirong, jinjide);

            Xingdong faqiXingdong = liucheng.ChuangjianXingdong("faqi", "开始", false, XingdongLeixing.Kaishi, neirong, null);
            Renwu renwu = faqiXingdong.ChuangjianRenwu(faqiren, "faqi");
            renwu.Chuli(faqiren, RenwuChuliJieguo.Tongguo, "");
            faqiXingdong.Wancheng(XingdongWanchengJieguo.Tongguo);

            List<User> zhixingren = new List<User>();
            zhixingren.Add(this.Yinqing.GetYonghu("user1"));
            zhixingren.Add(this.Yinqing.GetYonghu("user2"));
            zhixingren.Add(this.Yinqing.GetYonghu("user3"));
            Xingdong ceshiXingdong = liucheng.ChuangjianXingdong("ceshibuzhou", "测试步骤", false, XingdongLeixing.Jieshu, neirong, null);

            foreach (User yonghu in zhixingren)
            {
                ceshiXingdong.ChuangjianRenwu(yonghu, "ceshibuzhou");
            }
            return liucheng;
        }

        public void CeshiRenwuChuli(Renwu renwu, User chuliren, RenwuChuliJieguo jieguo, string shuoming, string neirong)
        {
            renwu.Chuli(chuliren, jieguo, shuoming);
            if (!renwu.Xingdong.RenwuList.Any(x => x.Zhuangtai == RenwuZhuangtai.Chulizhong))
            {
                int renwuTongguoCount = renwu.Xingdong.RenwuList.Count(x => x.ChuliJieguo.Value == RenwuChuliJieguo.Tongguo);
                if (renwuTongguoCount != renwu.Xingdong.RenwuList.Count)
                {
                    renwu.Xingdong.Wancheng(XingdongWanchengJieguo.Tuihui);
                    Liucheng liucheng = renwu.Xingdong.liucheng;
                    Xingdong tuihuiKaishiXingdong = liucheng.ChuangjianXingdong("ceshibuzhou", "开始", true, XingdongLeixing.Kaishi, neirong, null);
                    tuihuiKaishiXingdong.ChuangjianRenwu(liucheng.Faqiren, "faqi");
                }
                else
                {
                    renwu.Xingdong.Wancheng(XingdongWanchengJieguo.Tongguo);
                }
            }
        }

        public override byte[] ShengchengLiuchengtu(Liucheng liucheng)
        {
            Liuchengtu liuchengtu = new Liuchengtu(1050, 400);

            List<User> faqirenList = new List<User>();
            faqirenList.Add(liucheng.Faqiren);
            liuchengtu.HuaJiedian("开始", null, faqirenList, this.Yinqing.KaishiBuzhouImage);
            liuchengtu.HuaJiantou();

            Xingdong renwu = liucheng.XingdongList.Find(x => x.Bianhao == "ceshibuzhou");
            liuchengtu.HuaJiedian(renwu, this.Yinqing.BuzhouImage);
            liuchengtu.HuaJiantou();

            liuchengtu.HuaJiedian("结束", this.Yinqing.JieshuBuzhouImage);

            MemoryStream stream = new MemoryStream();
            liuchengtu.Save(stream, ImageFormat.Png);
            byte[] bytes = stream.ToArray();
            stream.Close();
            return bytes;
        }

        public override bool NengFaqi(User yong)
        {
            return true;
        }
    }
}
