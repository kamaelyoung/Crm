using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api.Workflow;
using Coldew.Data;
using Coldew.Core.Organization;

namespace Coldew.Core.Workflow
{
    public class Xingdong
    {

        public Xingdong(int id, string guid, string code, string name, bool jinjide, DateTime kaishiShjian,
            DateTime? qiwangWanchengShijian, DateTime? wanchengShijian, string zhaiyao, XingdongZhuangtai zhuangtai, LiuchengYinqing yingqing)
        {
            this.Code = code;
            this.Name = name;
            this.Id = id;
            this.Guid = guid;
            this.KaishiShijian = kaishiShjian;
            this.QiwangWanchengShijian = qiwangWanchengShijian;
            this.WanchengShijian = wanchengShijian;
            this.Zhaiyao = zhaiyao;
            this._renwuList = new List<Renwu>();
            this.Zhuangtai = zhuangtai;
            this.Jinjide = jinjide;
            this.Yingqing = yingqing;
        }
        public LiuchengYinqing Yingqing { protected set; get; }

        public int Id { protected set; get; }

        public string Guid { protected set; get; }

        public Liucheng liucheng { internal protected set; get; }

        public string Code { protected set; get; }

        public string Name { protected set; get; }

        public bool Jinjide { protected set; get; }

        List<Renwu> _renwuList;
        public List<Renwu> RenwuList
        {
            get
            {
                return _renwuList.ToList();
            }
        }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime KaishiShijian { protected set; get; }

        /// <summary>
        /// 期望完成时间
        /// </summary>
        public DateTime? QiwangWanchengShijian { protected set; get; }

        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime? WanchengShijian { protected set; get; }

        public string Zhaiyao { protected set; get; }

        public XingdongZhuangtai Zhuangtai { protected set; get; }

        private object _lock = new object();

        public Renwu ChuangjianRenwu(User chuliren)
        {
            lock (this._lock)
            {
                RenwuModel model = new RenwuModel();
                model.Guid = System.Guid.NewGuid().ToString();
                model.Chuliren = chuliren.Account;
                model.XingdongId = this.Id;
                model.Yongyouren = chuliren.Account;
                model.Zhuangtai = (int)RenwuZhuangtai.Chulizhong;
                model.Id = (int)NHibernateHelper.CurrentSession.Save(model);

                Renwu xingdong = this.ChuangjianRenwu(model);
                JianglaiRenwuZhipai jianglaiZhipai = this.Yingqing.JianglaiZhipaiManager.GetJaingLaiZhipai(chuliren);
                if (jianglaiZhipai != null && Helper.InDateRange(this.KaishiShijian, jianglaiZhipai.KaishiShijian, jianglaiZhipai.JieshuShijian))
                {
                    xingdong.Zhipai(jianglaiZhipai.Dailiren);
                }
                return xingdong;
            }
        }

        private Renwu ChuangjianRenwu(RenwuModel model)
        {
            User yongyouren = this.liucheng.Yinqing.GetYonghu(model.Yongyouren);
            User chuliren = this.liucheng.Yinqing.GetYonghu(model.Chuliren);
            RenwuChuliJieguo? chuliJieguo = null;
            if (model.ChuliJieguo.HasValue)
            {
                chuliJieguo = (RenwuChuliJieguo)model.ChuliJieguo.Value;
            }
            Renwu renwu = new Renwu(model.Id, model.Guid, yongyouren, chuliren, 
                model.ChuliShijian, (RenwuZhuangtai)model.Zhuangtai, chuliJieguo, model.ChuliShuoming, this);
            List<Renwu> renwuList = this.RenwuList;
            renwuList.Add(renwu);
            renwu.Shanchuhou += new TEventHanlder<Renwu>(Renwu_Shanchuhou);
            this._renwuList = renwuList;
            return renwu;
        }

        void Renwu_Shanchuhou(Renwu args)
        {
            List<Renwu> renwuList = this._renwuList.ToList();
            renwuList.Remove(args);
            this._renwuList = renwuList;
        }

        public virtual event TEventHanlder<Xingdong> Wanchenghou;

        public void Wancheng()
        {
            XingdongModel model = NHibernateHelper.CurrentSession.Get<XingdongModel>(this.Id);
            model.WanchengShijian = DateTime.Now;
            model.Zhuangtai = (int)XingdongZhuangtai.Wanchengle;
            NHibernateHelper.CurrentSession.Update(model);
            NHibernateHelper.CurrentSession.Flush();

            this.WanchengShijian = model.WanchengShijian;
            this.Zhuangtai = XingdongZhuangtai.Wanchengle;

            if (this.Wanchenghou != null)
            {
                this.Wanchenghou(this);
            }
        }

        public  virtual event TEventHanlder<Xingdong> Shanchuhou;

        public virtual void Shanchu(User shanchuren)
        {
            XingdongModel model = NHibernateHelper.CurrentSession.Get<XingdongModel>(this.Id);
            NHibernateHelper.CurrentSession.Delete(model);
            NHibernateHelper.CurrentSession.Flush();
            if (this.Shanchuhou != null)
            {
                this.Shanchuhou(this);
            }
        }

        internal void Jiazai()
        {
            List<RenwuModel> modellList = NHibernateHelper.CurrentSession.QueryOver<RenwuModel>().Where(x => x.XingdongId == this.Id).List().ToList();
            foreach (RenwuModel model in modellList)
            {
                this.ChuangjianRenwu(model);
            }
        }

        public XingdongXinxi Map()
        {
            return new XingdongXinxi
            {
                Code = this.Code,
                Name = this.Name,
                Id = this.Id,
                Guid = this.Guid,
                Jinjide = this.Jinjide,
                KaishiShijian = this.KaishiShijian,
                QiwangWanchengShijian = this.QiwangWanchengShijian,
                liucheng = this.liucheng.Map(),
                LiuchengMingcheng = this.liucheng.Mingcheng,
                WanchengShijian = this.WanchengShijian,
                Zhaiyao = this.Zhaiyao,
                Zhuangtai = this.Zhuangtai
            };
        }
    }
}
