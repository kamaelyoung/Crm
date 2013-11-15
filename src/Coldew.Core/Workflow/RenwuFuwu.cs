using System;
using System.Collections.Generic;
using System.Linq;
using Coldew.Api.Workflow;
using Coldew.Core.Organization;

namespace Coldew.Core.Workflow
{
    public class RenwuFuwu : IRenwuFuwu
    {
        LiuchengYinqing _yinqing;

        public RenwuFuwu(ColdewManager coldewManger)
        {
            this._yinqing = coldewManger.LiuchengYinqing;
        }

        public XingdongXinxi GetXingdong(string id)
        {
            Xingdong renwu = this._yinqing.LiuchengManager.GetXingdong(id);
            if (renwu != null)
            {
                return renwu.Map();
            }
            return null;
        }

        public RenwuXinxi GetRenwu(string liuchengId, string renwuId)
        {
            Liucheng liucheng = this._yinqing.LiuchengManager.GetLiucheng(liuchengId);
            Renwu renwu = liucheng.GetRenwu(renwuId);
            return renwu.Map();
        }

        public List<RenwuXinxi> GetChulizhongdeRenwu(string chulirenZhanghao, string mobanId, DateTime? kaishiShijian, DateTime? jieshuShijian, string zhaiyao, int start, int size, out int count)
        {
            return this._yinqing.LiuchengManager.GetChulizhongdeRenwu(chulirenZhanghao, mobanId, kaishiShijian, jieshuShijian, zhaiyao, start, size, out count)
                .Select(x => x.Map()).ToList();
        }

        public List<RenwuXinxi> GetWanchengdeRenwu(string chulirenZhanghao, string mobanId, DateTime? wanchengKaishiShijian, DateTime? wanchengJieshuShijian, string zhaiyao, int start, int size, out int count)
        {
            return this._yinqing.LiuchengManager.GetWanchengdeRenwu(chulirenZhanghao, mobanId, wanchengKaishiShijian, wanchengJieshuShijian, zhaiyao, start, size, out count)
                .Select(x => x.Map()).ToList();
        }

        public List<RenwuXinxi> GetGuidangdeRenwu(string chulirenZhanghao, string mobanId, DateTime? wanchengKaishiShijian, DateTime? wanchengJieshuShijian, string zhaiyao, int start, int size, out int count)
        {
            return this._yinqing.LiuchengManager.GetWanchengdeRenwu(chulirenZhanghao, mobanId, wanchengKaishiShijian, wanchengJieshuShijian, zhaiyao, start, size, out count)
                .Select(x => x.Map()).ToList();
        }

        public List<RenwuXinxi> GetLiuchengRenwu(string liuchengId)
        {
            Liucheng liucheng = this._yinqing.LiuchengManager.GetLiucheng(liuchengId);
            return liucheng.XingdongList.SelectMany(x => x.RenwuList).Select(x => x.Map()).ToList();
        }

        public List<RenwuXinxi> GetZhipaideRenwu(string zhipairenAccount, int start, int size, out int count)
        {
            User zhipairen = this._yinqing.GetYonghu(zhipairenAccount);
            List<Renwu> renwuList = this._yinqing.ZhipaiManager.GetZhipaideRenwuList(zhipairen);
            count = renwuList.Count;
            return renwuList.Skip(start).Take(size).Select(x => x.Map()).ToList();
        }

        public void XiugaiRenwuChuliren(string renwuId, string chulirenZhanghao)
        {
            User yonghu = this._yinqing.GetYonghu(chulirenZhanghao);
            Renwu renwu = this._yinqing.LiuchengManager.GetRenwu(renwuId);
            renwu.XiugaiChuliren(yonghu);
        }

        public void ZhipaiRenwu(string[] renwuId, string yonghuZhanghao)
        {
            User yonghu = this._yinqing.GetYonghu(yonghuZhanghao);
            foreach (string id in renwuId)
            {
                Renwu renwu = this._yinqing.LiuchengManager.GetRenwu(id);
                renwu.Zhipai(yonghu);
            }
        }

        public void QuxiaoZhipai(string[] renwuId)
        {
            foreach (string id in renwuId)
            {
                Renwu renwu = this._yinqing.LiuchengManager.GetRenwu(id);
                renwu.QuxiaoZhipai();
            }
        }

        public void ZhipaiSuoyouRenwu(string zhipairenZhanghao, string dailirenZhanghao)
        {
            this._yinqing.LiuchengManager.ZhipaiSuoyouRenwu(zhipairenZhanghao, dailirenZhanghao);
        }

        public void SetJianglaiZhipai(string zhipairenZhanghao, string dailirenZhanghao, DateTime? kaishiShijian, DateTime? jieshuShijian)
        {
            User zhipairen = this._yinqing.GetYonghu(zhipairenZhanghao);
            User dailiren = this._yinqing.GetYonghu(dailirenZhanghao);
            this._yinqing.JianglaiZhipaiManager.SetJianglaiRenwuZhipai(zhipairen, dailiren, kaishiShijian, jieshuShijian);
        }

        public JianglaiZhipaiXinxi GetJianglaiZhipai(string zhipairenZhanghao)
        {
            User zhipairen = this._yinqing.GetYonghu(zhipairenZhanghao);
            JianglaiRenwuZhipai zhipai = this._yinqing.JianglaiZhipaiManager.GetJaingLaiZhipai(zhipairen);
            if (zhipai != null)
            {
                return zhipai.Map();
            }
            return null;
        }

        public RenwuXinxi WanchengRenwu(string liuchengId, string chulirenAccount, string renwuId, string shuoming)
        {
            Liucheng liucheng = this._yinqing.LiuchengManager.GetLiucheng(liuchengId);
            Renwu renwu = liucheng.GetRenwu(renwuId);
            User chuliren = this._yinqing.GetYonghu(chulirenAccount);
            renwu.Wancheng(chuliren, shuoming);
            return renwu.Map();
        }

        public XingdongXinxi ChuangjianXingdong(string liuchengId, string code, string name, List<string> chulirenAccounts, string zhaiyao, DateTime? qiwangWanchengShijian)
        {
            Liucheng liucheng = this._yinqing.LiuchengManager.GetLiucheng(liuchengId);
            Xingdong xingdong = liucheng.ChuangjianXingdong(code, name, zhaiyao, qiwangWanchengShijian);
            foreach (string account in chulirenAccounts)
            {
                User user = this._yinqing.GetYonghu(account);
                xingdong.ChuangjianRenwu(user);
            }
            return xingdong.Map();
        }

        public void WanchengXingdong(string liuchengId, string xingdongId)
        {
            Liucheng liucheng = this._yinqing.LiuchengManager.GetLiucheng(liuchengId);
            Xingdong xingdong = liucheng.GetXingdong(xingdongId);
            xingdong.Wancheng();
        }
    }
}
