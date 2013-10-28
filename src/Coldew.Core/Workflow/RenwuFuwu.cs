using System;
using System.Collections.Generic;
using System.Linq;
using Coldew.Api.Workflow;
using Coldew.Core.Organization;

namespace Coldew.Core.Workflow
{
    public class RenwuFuwu : IRenwuFuwu
    {
        Yinqing _yinqing;

        public RenwuFuwu(ColdewManager coldewManger)
        {
            this._yinqing = coldewManger.LiuchengYinqing;
        }

        public XingdongXinxi GetXingdong(string id)
        {
            Xingdong renwu = this._yinqing.GetXingdong(id);
            if (renwu != null)
            {
                return renwu.Map();
            }
            return null;
        }

        public RenwuXinxi GetRenwu(string renwuId)
        {
            foreach (LiuchengMoban liucheng in this._yinqing.LiuchengMobanList)
            {
                Renwu renwu = liucheng.GetRenwu(renwuId);
                if (renwu != null)
                {
                    return renwu.Map();
                }
            }
            return null;
        }

        public List<RenwuXinxi> GetChulizhongdeRenwu(string chulirenZhanghao, string mobanId, DateTime? kaishiShijian, DateTime? jieshuShijian, string zhaiyao, int start, int size, out int count)
        {
            User chuliren = this._yinqing.GetYonghu(chulirenZhanghao);
            List<Liucheng> liuchengList = this._yinqing.LiuchengMobanList.SelectMany(x => x.LiuchengList).ToList();
            List<Renwu> renwuList = new List<Renwu>();

            if (zhaiyao == null)
            {
                zhaiyao = "";
            }
            foreach (Liucheng liucheng in liuchengList)
            {
                if (!string.IsNullOrEmpty(mobanId) && liucheng.Moban.Guid != mobanId)
                {
                    continue;
                }

                renwuList.AddRange(liucheng.XingdongList.Where(x => x.Zhuangtai == XingdongZhuangtai.Chulizhong && Helper.InDateRange(x.KaishiShijian, kaishiShijian, jieshuShijian) && x.Zhaiyao.IndexOf(zhaiyao, StringComparison.InvariantCultureIgnoreCase) > -1 )
                    .SelectMany(x => x.RenwuList.Where(xd => xd.Zhuangtai == RenwuZhuangtai.Chulizhong && xd.NengChuli(chuliren))));
            }
            count = renwuList.Count;
            return renwuList.Skip(start).Take(size).Select(x => x.Map()).ToList();
        }

        public List<RenwuXinxi> GetWanchengdeRenwu(string chulirenZhanghao, string mobanId, DateTime? wanchengKaishiShijian, DateTime? wanchengJieshuShijian, string zhaiyao, int start, int size, out int count)
        {
            if (zhaiyao == null)
            {
                zhaiyao = "";
            }
            User chuliren = this._yinqing.GetYonghu(chulirenZhanghao);
            List<Liucheng> shiliList = this._yinqing.LiuchengMobanList.SelectMany(x => x.LiuchengList).ToList();
            List<Renwu> renwuList = new List<Renwu>();
            foreach (Liucheng liucheng in shiliList)
            {
                if (!string.IsNullOrEmpty(mobanId) && liucheng.Moban.Guid != mobanId)
                {
                    continue;
                }
                if (liucheng.Zhuangtai == LiuchengZhuangtai.Chulizhong)
                {
                    var shiliRenwuList = liucheng.XingdongList.SelectMany(xingdong =>
                    {
                        return xingdong.RenwuList.Where(renwu =>
                        {
                            if (renwu.Zhuangtai != RenwuZhuangtai.Wanchengle)
                            {
                                return false;
                            }
                            if (!Helper.InDateRange(renwu.ChuliShijian.Value, wanchengKaishiShijian, wanchengJieshuShijian))
                            {
                                return false;
                            }
                            if (xingdong.Zhaiyao.IndexOf(zhaiyao, StringComparison.InvariantCultureIgnoreCase) == -1)
                            {
                                return false;
                            }
                            if (renwu.Chuliren.Equals(chuliren))
                            {
                                return true;
                            }
                            if (renwu.ShijiChuliren != null && renwu.ShijiChuliren.Equals(chuliren))
                            {
                                return true;
                            }
                            return false;
                        });
                    });
                    renwuList.AddRange(shiliRenwuList);
                }
            }
            count = renwuList.Count;
            return renwuList.Skip(start).Take(size).Select(x => x.Map()).ToList();
        }

        public List<RenwuXinxi> GetGuidangdeRenwu(string chulirenZhanghao, string mobanId, DateTime? wanchengKaishiShijian, DateTime? wanchengJieshuShijian, string zhaiyao, int start, int size, out int count)
        {
            if (zhaiyao == null)
            {
                zhaiyao = "";
            }
            User chuliren = this._yinqing.GetYonghu(chulirenZhanghao);
            List<Liucheng> shiliList = this._yinqing.LiuchengMobanList.SelectMany(x => x.LiuchengList).ToList();
            List<Renwu> renwuList = new List<Renwu>();
            foreach (Liucheng liucheng in shiliList)
            {
                if (!string.IsNullOrEmpty(mobanId) && liucheng.Moban.Guid != mobanId)
                {
                    continue;
                }
                if (liucheng.Zhuangtai == LiuchengZhuangtai.Wanchengle)
                {
                    var shiliRenwuList = liucheng.XingdongList.SelectMany(xingdong =>
                    {
                        return xingdong.RenwuList.Where(renwu =>
                        {
                            if (renwu.Zhuangtai != RenwuZhuangtai.Wanchengle)
                            {
                                return false;
                            }
                            if (!Helper.InDateRange(renwu.ChuliShijian.Value, wanchengKaishiShijian, wanchengJieshuShijian))
                            {
                                return false;
                            }
                            if (xingdong.Zhaiyao.IndexOf(zhaiyao, StringComparison.InvariantCultureIgnoreCase) == -1)
                            {
                                return false;
                            }
                            if (renwu.Chuliren.Equals(chuliren))
                            {
                                return true;
                            }
                            if (renwu.ShijiChuliren != null && renwu.ShijiChuliren.Equals(chuliren))
                            {
                                return true;
                            }
                            return false;
                        });
                    });
                    renwuList.AddRange(shiliRenwuList);
                }
            }
            count = renwuList.Count;
            return renwuList.Skip(start).Take(size).Select(x => x.Map()).ToList();
        }

        public List<RenwuXinxi> GetFaqideRenwu(string chulirenZhanghao, string mobanId, DateTime? kaishiShijian, DateTime? jieshuShijian, string zhaiyao, int start, int size, out int count)
        {
            if (zhaiyao == null)
            {
                zhaiyao = "";
            }
            User chuliren = this._yinqing.GetYonghu(chulirenZhanghao);
            List<Liucheng> shiliList = this._yinqing.LiuchengMobanList.SelectMany(x => x.LiuchengList).ToList();
            List<Renwu> renwuList = new List<Renwu>();
            foreach (Liucheng liucheng in shiliList)
            {
                if (!string.IsNullOrEmpty(mobanId) && liucheng.Moban.Guid != mobanId)
                {
                    continue;
                }
                if(!Helper.InDateRange(liucheng.FaqiShijian, kaishiShijian, jieshuShijian))
                {
                    continue;
                }
                var shiliRenwuList = liucheng.XingdongList.Where(xingdong =>
                {
                    if (xingdong.Tuihuide)
                    {
                        return false;
                    }
                    if (xingdong.Leixing != XingdongLeixing.Kaishi)
                    {
                        return false;
                    }
                    if (xingdong.Zhaiyao.IndexOf(zhaiyao, StringComparison.InvariantCultureIgnoreCase) == -1)
                    {
                        return false;
                    }
                    if (!xingdong.liucheng.Faqiren.Equals(chuliren))
                    {
                        return false;
                    }
                    return true;
                }).SelectMany(xingdong => xingdong.RenwuList);
                renwuList.AddRange(shiliRenwuList);
            }
            count = renwuList.Count;
            return renwuList.Skip(start).Take(size).Select(x => x.Map()).ToList();
        }

        public List<RenwuXinxi> GetLiuchengRenwu(string liuchengId)
        {
            Liucheng liucheng = this._yinqing.GetLiucheng(liuchengId);
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
            Renwu renwu = this._yinqing.GetRenwu(renwuId);
            renwu.XiugaiChuliren(yonghu);
        }

        public void ZhipaiRenwu(string[] renwuId, string yonghuZhanghao)
        {
            User yonghu = this._yinqing.GetYonghu(yonghuZhanghao);
            foreach (string id in renwuId)
            {
                Renwu renwu = this._yinqing.GetRenwu(id);
                renwu.Zhipai(yonghu);
            }
        }

        public void QuxiaoZhipai(string[] renwuId)
        {
            foreach (string id in renwuId)
            {
                Renwu renwu = this._yinqing.GetRenwu(id);
                renwu.QuxiaoZhipai();
            }
        }

        public void ZhipaiSuoyouRenwu(string zhipairenZhanghao, string dailirenZhanghao)
        {
            User zhipairen = this._yinqing.GetYonghu(zhipairenZhanghao);
            User dailiren = this._yinqing.GetYonghu(dailirenZhanghao);

            List<Liucheng> liuchengList = this._yinqing.LiuchengMobanList.SelectMany(x => x.LiuchengList).ToList();
            List<Renwu> renwuList = new List<Renwu>();
            foreach (Liucheng liucheng in liuchengList)
            {
                renwuList.AddRange(liucheng.XingdongList.Where(x => x.Zhuangtai == XingdongZhuangtai.Chulizhong)
                    .SelectMany(x => x.RenwuList.Where(xd => xd.Zhuangtai == RenwuZhuangtai.Chulizhong && xd.NengChuli(zhipairen))));
            }
            foreach (Renwu renwu in renwuList)
            {
                renwu.Zhipai(dailiren);
            }
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
    }
}
