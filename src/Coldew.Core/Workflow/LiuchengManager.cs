using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core.Organization;
using Coldew.Data;
using Coldew.Api.Workflow;
using System.Threading;

namespace Coldew.Core.Workflow
{
    public class LiuchengManager
    {

        Dictionary<string, Liucheng> _shiliDictionaryById;
        List<Liucheng> _shiliList;
        UserManagement _userManager;
        LiuchengYinqing _yinqing;
        protected ReaderWriterLock _lock;

        public LiuchengManager(LiuchengYinqing yingqing)
        {
            this._shiliList = new List<Liucheng>();
            this._lock = new ReaderWriterLock();

            this._userManager = yingqing.ColdewManager.OrgManager.UserManager;
            this._yinqing = yingqing;
        }

        public List<Liucheng> GetLiuchengList(string liuchengMobanId, ShijianFanwei faqiShijianFanwei, ShijianFanwei jieshuShijianFanwei, string zhaiyao, int start, int size, out int count)
        {
            this._lock.AcquireReaderLock(0);
            try
            {
                List<Liucheng> liuchengList = new List<Liucheng>();
                foreach (Liucheng liucheng in this._shiliList)
                {
                    if (!string.IsNullOrEmpty(liuchengMobanId) && liucheng.Moban.ID != liuchengMobanId)
                    {
                        continue;
                    }
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
                count = liuchengList.Count;
                return liuchengList.Skip(start).Take(size).ToList();
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public Liucheng FaqiLiucheng(User faqiren, string mobanId, string zhaiyao, bool jinjide, Metadata biaodan)
        {
            this._lock.AcquireWriterLock(0);
            try
            {
                LiuchengMoban moban = this._yinqing.LiuchengMobanManager.GetMobanById(mobanId);

                LiuchengModel model = new LiuchengModel();
                model.Guid = System.Guid.NewGuid().ToString();
                model.Faqiren = faqiren.Account;
                model.FaqiShijian = DateTime.Now;
                model.Mingcheng = moban.Mingcheng;
                model.MobanId = mobanId;
                model.Zhuangtai = (int)LiuchengZhuangtai.Chulizhong;
                model.Zhaiyao = zhaiyao;
                model.Jinjide = jinjide;
                model.BiaodanId = biaodan.ID;
                int id = (int)NHibernateHelper.CurrentSession.Save(model);
                Liucheng liucheng = this.ChuangjianLiucheng(model);
                return liucheng;
            
            }
            finally
            {
                this._lock.ReleaseWriterLock();
            }
        }

        internal Liucheng ChuangjianLiucheng(LiuchengModel model)
        {
            LiuchengMoban moban = this._yinqing.LiuchengMobanManager.GetMobanById(model.MobanId);
            Metadata biaodan = moban.ColdewObject.MetadataManager.GetById(model.BiaodanId);
            Liucheng liucheng = new Liucheng(model.Id, model.Guid, model.Mingcheng, this._userManager.GetUserByAccount(model.Faqiren),
                model.FaqiShijian, model.JieshuShijian, (LiuchengZhuangtai)model.Zhuangtai, model.Jinjide, model.Zhaiyao, biaodan, this._yinqing);
            liucheng.Shanchuhou += new TEventHanlder<Liucheng>(Liucheng_Shanchuhou);
            List<Liucheng> shiliList = this._shiliList.ToList();
            liucheng.Moban = this._yinqing.LiuchengMobanManager.GetMobanById(model.MobanId);
            shiliList.Add(liucheng);
            this._shiliList = shiliList;
            this.Suoyin();
            return liucheng;
        }

        public void Liucheng_Shanchuhou(Liucheng liucheng)
        {
            List<Liucheng> shiliList = this._shiliList.ToList();
            shiliList.Remove(liucheng);
            this._shiliList = shiliList;
            this.Suoyin();
        }

        private void Suoyin()
        {
            this._shiliDictionaryById = this._shiliList.ToDictionary(x => x.Guid);
        }

        public Liucheng GetLiucheng(string id)
        {
            this._lock.AcquireReaderLock(0);
            try
            {
                if (this._shiliDictionaryById.ContainsKey(id))
                {
                    return this._shiliDictionaryById[id];
                }
                return null;
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public List<Renwu> GetChulizhongdeRenwu(string chulirenZhanghao, string mobanId, DateTime? kaishiShijian, DateTime? jieshuShijian, string zhaiyao, int start, int size, out int count)
        {
            User chuliren = this._yinqing.GetYonghu(chulirenZhanghao);
            List<Renwu> renwuList = new List<Renwu>();

            if (zhaiyao == null)
            {
                zhaiyao = "";
            }
            foreach (Liucheng liucheng in this._shiliList)
            {
                if (!string.IsNullOrEmpty(mobanId) && liucheng.Moban.ID != mobanId)
                {
                    continue;
                }

                renwuList.AddRange(liucheng.XingdongList.Where(x => x.Zhuangtai == XingdongZhuangtai.Chulizhong && Helper.InDateRange(x.KaishiShijian, kaishiShijian, jieshuShijian) && x.Zhaiyao.IndexOf(zhaiyao, StringComparison.InvariantCultureIgnoreCase) > -1)
                    .SelectMany(x => x.RenwuList.Where(xd => xd.Zhuangtai == RenwuZhuangtai.Chulizhong && xd.NengChuli(chuliren))));
            }
            count = renwuList.Count;
            return renwuList.Skip(start).Take(size).ToList();
        }

        public List<Renwu> GetChulizhongdeRenwu()
        {
            List<Renwu> renwuList = new List<Renwu>();
            foreach (Liucheng liucheng in this._shiliList)
            {
                renwuList.AddRange(liucheng.XingdongList.SelectMany(x => x.RenwuList.Where(xd => xd.Zhuangtai == RenwuZhuangtai.Chulizhong)));
            }
            return renwuList;
        }

        public List<Renwu> GetWanchengdeRenwu(string chulirenZhanghao, string mobanId, DateTime? wanchengKaishiShijian, DateTime? wanchengJieshuShijian, string zhaiyao, int start, int size, out int count)
        {
            if (zhaiyao == null)
            {
                zhaiyao = "";
            }
            User chuliren = this._yinqing.GetYonghu(chulirenZhanghao);
            List<Renwu> renwuList = new List<Renwu>();
            foreach (Liucheng liucheng in this._shiliList)
            {
                if (!string.IsNullOrEmpty(mobanId) && liucheng.Moban.ID != mobanId)
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
                            return false;
                        });
                    });
                    renwuList.AddRange(shiliRenwuList);
                }
            }
            count = renwuList.Count;
            return renwuList.Skip(start).Take(size).ToList();
        }

        public List<Renwu> GetGuidangdeRenwu(string chulirenZhanghao, string mobanId, DateTime? wanchengKaishiShijian, DateTime? wanchengJieshuShijian, string zhaiyao, int start, int size, out int count)
        {
            if (zhaiyao == null)
            {
                zhaiyao = "";
            }
            User chuliren = this._yinqing.GetYonghu(chulirenZhanghao);
            List<Renwu> renwuList = new List<Renwu>();
            foreach (Liucheng liucheng in this._shiliList)
            {
                if (!string.IsNullOrEmpty(mobanId) && liucheng.Moban.ID != mobanId)
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
                            return false;
                        });
                    });
                    renwuList.AddRange(shiliRenwuList);
                }
            }
            count = renwuList.Count;
            return renwuList.Skip(start).Take(size).ToList();
        }

        public Renwu GetRenwu(string renwuId)
        {
            foreach (Liucheng liucheng in this._shiliList)
            {
                foreach (Xingdong renwu in liucheng.XingdongList)
                {
                    Renwu xingdong = renwu.RenwuList.Find(x => x.Guid == renwuId);
                    if (xingdong != null)
                    {
                        return xingdong;
                    }
                }
            }
            return null;
        }

        public Xingdong GetXingdong(string guid)
        {
            foreach (Liucheng liucheng in this._shiliList)
            {
                Xingdong renwu = liucheng.GetXingdong(guid);
                if (renwu != null)
                {
                    return renwu;
                }
            }
            return null;
        }

        public void ZhipaiSuoyouRenwu(string zhipairenZhanghao, string dailirenZhanghao)
        {
            User zhipairen = this._yinqing.GetYonghu(zhipairenZhanghao);
            User dailiren = this._yinqing.GetYonghu(dailirenZhanghao);

            List<Renwu> renwuList = new List<Renwu>();
            foreach (Liucheng liucheng in this._shiliList)
            {
                renwuList.AddRange(liucheng.XingdongList.Where(x => x.Zhuangtai == XingdongZhuangtai.Chulizhong)
                    .SelectMany(x => x.RenwuList.Where(xd => xd.Zhuangtai == RenwuZhuangtai.Chulizhong && xd.NengChuli(zhipairen))));
            }
            foreach (Renwu renwu in renwuList)
            {
                renwu.Zhipai(dailiren);
            }
        }

        internal protected virtual void Jiazai()
        {
            List<LiuchengModel> shiliModelList = NHibernateHelper.CurrentSession.QueryOver<LiuchengModel>().List().ToList();
            foreach (LiuchengModel shiliModel in shiliModelList)
            {
                Liucheng liucheng = this.ChuangjianLiucheng(shiliModel);
                liucheng.Jiazai();
            }
        }
    }
}
