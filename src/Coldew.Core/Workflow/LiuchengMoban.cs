using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Data;
using Coldew.Api.Workflow;
using System.IO;
using System.Drawing.Imaging;
using Coldew.Core.Organization;

namespace Coldew.Core.Workflow
{
    public abstract class LiuchengMoban
    {
        public LiuchengMoban(int id, string guid, string mingcheng, string faqiUrl, string renwuUrl, string guidangUrl, string shuoming)
        {
            this.Id = id;
            this.Guid = guid;
            this.Mingcheng = mingcheng;
            this.Shuoming = shuoming;
            this.FaqiUrl = faqiUrl;
            this.RenwuUrl = renwuUrl;
            this.GuidangUrl = guidangUrl;
            this._shiliList = new List<Liucheng>();
        }

        public Yinqing Yinqing { internal set; get; }

        public int Id { protected set; get; }

        public string Guid { protected set; get; }

        public string Mingcheng { protected set; get; }

        public string FaqiUrl { protected set; get; }

        public string RenwuUrl { protected set; get; }

        public string GuidangUrl { protected set; get; }

        public string Shuoming { protected set; get; }
        
        Dictionary<string, Liucheng> _shiliDictionaryById;
        List<Liucheng> _shiliList;
        public List<Liucheng> LiuchengList
        {
            get
            {
                return this._shiliList.ToList();
            }
        }

        private object _lock = new object();

        public Liucheng FaqiLiucheng(User faqiren, string zhaiyao, bool jinjide)
        {
            lock (this._lock)
            {
                LiuchengModel model = new LiuchengModel();
                model.Guid = System.Guid.NewGuid().ToString();
                model.Faqiren = faqiren.Account;
                model.FaqiShijian = DateTime.Now;
                model.Mingcheng = this.Mingcheng;
                model.MobanId = this.Id;
                model.Zhuangtai = (int)LiuchengZhuangtai.Chulizhong;
                model.Zhaiyao = zhaiyao;
                model.Jinjide = jinjide;
                int id = (int)NHibernateHelper.CurrentSession.Save(model);
                Liucheng liucheng = this.ChuangjianLiucheng(model);
                return liucheng;
            }
        }

        public abstract byte[] ShengchengLiuchengtu(Liucheng liucheng);

        public abstract bool NengFaqi(User yong);

        internal Liucheng ChuangjianLiucheng(LiuchengModel model)
        {
            Liucheng liucheng = new Liucheng(model.Id, model.Guid, model.Mingcheng, this.Yinqing.GetYonghu(model.Faqiren), 
                model.FaqiShijian, model.JieshuShijian, (LiuchengZhuangtai)model.Zhuangtai, model.Jinjide, model.Zhaiyao, this.Yinqing);
            liucheng.Shanchuhou += new TEventHanlder<Liucheng>(Liucheng_Shanchuhou);
            List<Liucheng> shiliList = this._shiliList.ToList();
            liucheng.Moban = this;
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
            if (this._shiliDictionaryById.ContainsKey(id))
            {
                return this._shiliDictionaryById[id];
            }
            return null;
        }

        public Liucheng GetLiucheng(int id)
        {
            return this._shiliList.Find(x => x.Id == id);
        }

        public Xingdong GetXingdong(int id)
        {
            foreach (Liucheng liucheng in this._shiliList)
            {
                Xingdong renwu = liucheng.GetRenwu(id);
                if (renwu != null)
                {
                    return renwu;
                }
            }
            return null;
        }

        public Xingdong GetXingdong(string id)
        {
            foreach (Liucheng liucheng in this._shiliList)
            {
                Xingdong renwu = liucheng.GetRenwu(id);
                if (renwu != null)
                {
                    return renwu;
                }
            }
            return null;
        }

        public Renwu GetRenwu(string id)
        {
            foreach (Liucheng liucheng in this._shiliList)
            {
                foreach (Xingdong renwu in liucheng.XingdongList)
                {
                    Renwu xingdong = renwu.RenwuList.Find(x => x.Guid == id);
                    if (xingdong != null)
                    {
                        return xingdong;
                    }
                }
            }
            return null;
        }

        internal protected virtual void Jiazai()
        {
            this.JiazaiLiucheng();
        }

        private void JiazaiLiucheng()
        {
            List<LiuchengModel> shiliModelList = NHibernateHelper.CurrentSession.QueryOver<LiuchengModel>().Where(x => x.MobanId == this.Id).List().ToList();
            foreach (LiuchengModel shiliModel in shiliModelList)
            {
                Liucheng liucheng = this.ChuangjianLiucheng(shiliModel);
                liucheng.Jiazai();
            }
        }

        public LiuchengMobanXinxi Map()
        {
            return new LiuchengMobanXinxi
            {
                Id = this.Id,
                Guid = this.Guid,
                Mingcheng = this.Mingcheng,
                Shuoming = this.Shuoming,
                FaqiUrl = this.FaqiUrl,
                RenwuUrl = this.RenwuUrl,
                GuidangUrl = this.GuidangUrl
            };
        }
    }
}
