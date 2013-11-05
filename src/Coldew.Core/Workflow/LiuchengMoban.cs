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
    public class LiuchengMoban
    {

        public LiuchengMoban(string id, string code, string mingcheng, string transferUrl, string shuoming, LiuchengYinqing yinqing, ColdewObject cobject)
        {
            this.ID = id;
            this.Mingcheng = mingcheng;
            this.TransferUrl = transferUrl;
            this.Shuoming = shuoming;
            this.Yinqing = yinqing;
            this.BiandanManager = cobject.MetadataManager;
            this._shiliList = new List<Liucheng>();
        }

        public MetadataManager BiandanManager { protected set; get; }

        public LiuchengYinqing Yinqing { internal set; get; }

        public string ID { protected set; get; }

        public string Code { protected set; get; }

        public string Mingcheng { protected set; get; }

        public string TransferUrl { protected set; get; }

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

        public Liucheng FaqiLiucheng(User faqiren, string zhaiyao, bool jinjide, Metadata biaodan)
        {
            lock (this._lock)
            {
                LiuchengModel model = new LiuchengModel();
                model.Guid = System.Guid.NewGuid().ToString();
                model.Faqiren = faqiren.Account;
                model.FaqiShijian = DateTime.Now;
                model.Mingcheng = this.Mingcheng;
                model.MobanId = this.ID;
                model.Zhuangtai = (int)LiuchengZhuangtai.Chulizhong;
                model.Zhaiyao = zhaiyao;
                model.Jinjide = jinjide;
                model.BiaodanId = biaodan.ID;
                int id = (int)NHibernateHelper.CurrentSession.Save(model);
                Liucheng liucheng = this.ChuangjianLiucheng(model);
                return liucheng;
            }
        }

        public bool NengFaqi(User yong)
        {
            return true;
        }

        internal Liucheng ChuangjianLiucheng(LiuchengModel model)
        {
            Metadata biaodan = this.BiandanManager.GetById(model.BiaodanId);
            Liucheng liucheng = new Liucheng(model.Id, model.Guid, model.Mingcheng, this.Yinqing.GetYonghu(model.Faqiren),
                model.FaqiShijian, model.JieshuShijian, (LiuchengZhuangtai)model.Zhuangtai, model.Jinjide, model.Zhaiyao, biaodan, this.Yinqing);
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
                Xingdong renwu = liucheng.GetXingdong(id);
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
                Xingdong renwu = liucheng.GetXingdong(id);
                if (renwu != null)
                {
                    return renwu;
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
            List<LiuchengModel> shiliModelList = NHibernateHelper.CurrentSession.QueryOver<LiuchengModel>().Where(x => x.MobanId == this.ID).List().ToList();
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
                ID = this.ID,
                Mingcheng = this.Mingcheng,
                Code = this.Code,
                TransferUrl = this.TransferUrl,
                Shuoming = this.Shuoming
            };
        }
    }
}
