using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Coldew.Data;
using Coldew.Api.Workflow;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using Coldew.Api.Workflow.Exceptions;
using Coldew.Core.Organization;

namespace Coldew.Core.Workflow
{
    public class Liucheng
    {
        public Liucheng(int id, string guid, string mingcheng, User faqiren, DateTime faqiShijian, 
            DateTime? jieshuShijian, LiuchengZhuangtai zhuangtai, bool jinjide, string zhaiyao, Yinqing yinqing)
        {
            this.Id = id;
            this.Guid = guid;
            this.Mingcheng = mingcheng;
            this.Faqiren = faqiren;
            this.FaqiShijian = faqiShijian;
            this.JieshuShijian = jieshuShijian;
            this.Zhuangtai = zhuangtai;
            this.Zhaiyao = zhaiyao;
            this.Jinjide = jinjide;
            this._xingdongList = new List<Xingdong>();
            this.Yinqing = yinqing;
        }

        public Yinqing Yinqing { private set; get; }

        private object _lock = new object();

        public int Id { private set; get; }

        public string Guid { private set; get; }

        public LiuchengMoban Moban { internal set; get; }

        public string Mingcheng { private set; get; }

        public User Faqiren { private set; get; }

        public DateTime FaqiShijian { private set; get; }

        public DateTime? JieshuShijian { private set; get; }

        public LiuchengZhuangtai Zhuangtai { private set; get; }

        public bool Jinjide { private set; get; }

        public string Zhaiyao { private set; get; }

        List<Xingdong> _xingdongList;
        public List<Xingdong> XingdongList
        {
            get
            {
                return this._xingdongList.ToList();
            }
        }

        public event TEventHanlder<Liucheng, Xingdong> XingdongChuangjianhou;

        public Xingdong ChuangjianXingdong(string bianhao, string mingcheng, bool tuihuide, XingdongLeixing leixing, string zhaiyao, DateTime? qiwangWanchengShijian)
        {
            lock (_lock)
            {
                XingdongModel model = new XingdongModel();
                model.LiuchengId = this.Id;
                model.Guid = System.Guid.NewGuid().ToString();
                model.Bianhao = bianhao;
                model.KaishiShijian = DateTime.Now;
                model.Tuihuide = tuihuide;
                model.Mingcheng = mingcheng;
                model.QiwangWanchengShijian = qiwangWanchengShijian;
                model.Zhaiyao = zhaiyao;
                model.Jinjide = this.Jinjide;
                model.Zhuangtai = (int)XingdongZhuangtai.Chulizhong;
                model.Leixing = (int)leixing;
                int id = (int)NHibernateHelper.CurrentSession.Save(model);
                Xingdong renwu = this.ChuangjianXingdong(model);
                if (this.XingdongChuangjianhou != null)
                {
                    this.XingdongChuangjianhou(this, renwu);
                }

                return renwu;
            }
        }

        internal Xingdong ChuangjianXingdong(XingdongModel model)
        {
            XingdongWanchengJieguo? wanchengJieguo = null;
            if (model.WanchengJieguo.HasValue)
            {
                wanchengJieguo = (XingdongWanchengJieguo)model.WanchengJieguo.Value;
            }
            Xingdong xingdong = new Xingdong(model.Id, model.Guid, model.Bianhao, model.Mingcheng, model.Jinjide, model.Tuihuide,
                    model.KaishiShijian, model.QiwangWanchengShijian, model.WanchengShijian, model.Zhaiyao, 
                    (XingdongZhuangtai)model.Zhuangtai, wanchengJieguo, (XingdongLeixing)model.Leixing, this.Yinqing);
            xingdong.Shanchuhou += new TEventHanlder<Xingdong>(Xingdong_Shanchuhou);
            List<Xingdong> renwuList = this._xingdongList.ToList();
            xingdong.liucheng = this;
            xingdong.Wanchenghou += new TEventHanlder<Xingdong>(Renwu_Wanchenghou);
            renwuList.Add(xingdong);
            this._xingdongList = renwuList;
            return xingdong;
        }

        void Renwu_Wanchenghou(Xingdong xingdong)
        {
            if (xingdong.Leixing == XingdongLeixing.Jieshu && xingdong.WanchengJieguo.Value == XingdongWanchengJieguo.Tongguo)
            {
                this.Wancheng();
            }
        }

        public Xingdong GetRenwu(int id)
        {
            return this._xingdongList.Find(x => x.Id == id);
        }

        public Xingdong GetRenwu(string guid)
        {
            return this._xingdongList.Find(x => x.Guid == guid);
        }

        void Xingdong_Shanchuhou(Xingdong args)
        {
            List<Xingdong> renwuList = this._xingdongList.ToList();
            renwuList.Remove(args);
            this._xingdongList = renwuList;
        }

        public void XiugaiZhaiyao(string zhaiyao)
        {
            LiuchengModel model = NHibernateHelper.CurrentSession.Get<LiuchengModel>(this.Id);
            model.Zhaiyao = zhaiyao;
            NHibernateHelper.CurrentSession.Update(model);
            NHibernateHelper.CurrentSession.Flush();

            this.Zhaiyao = zhaiyao;
        }

        public void Wancheng()
        {
            LiuchengModel model = NHibernateHelper.CurrentSession.Get<LiuchengModel>(this.Id);
            model.JieshuShijian = DateTime.Now;
            model.Zhuangtai = (int)LiuchengZhuangtai.Wanchengle;
            NHibernateHelper.CurrentSession.Update(model);
            NHibernateHelper.CurrentSession.Flush();

            this.JieshuShijian = model.JieshuShijian;
            this.Zhuangtai = LiuchengZhuangtai.Wanchengle;
        }

        public void Chexiao()
        {
            Xingdong xingdong = this._xingdongList.Find(x => x.Zhuangtai == XingdongZhuangtai.Chulizhong);
            if (xingdong.Leixing == XingdongLeixing.Kaishi)
            {
                this.Shanchu(this.Faqiren);
            }
            else if (this._xingdongList.Count == 2 && xingdong.RenwuList.Count(x => x.Zhuangtai == RenwuZhuangtai.Wanchengle) == 0)
            {
                this.Shanchu(this.Faqiren);
            }
            else
            {
                throw new LiuchengChexiaoException();
            }
        }

        public event TEventHanlder<Liucheng> Shanchuhou;

        public void Shanchu(User shanchuren)
        {
            this._xingdongList.ForEach(x => x.Shanchu(shanchuren));

            LiuchengModel model = NHibernateHelper.CurrentSession.Get<LiuchengModel>(this.Id);
            NHibernateHelper.CurrentSession.Delete(model);
            NHibernateHelper.CurrentSession.Flush();

            if (this.Shanchuhou != null)
            {
                this.Shanchuhou(this);
            }
        }

        public byte[] GetLiuchengtu()
        {
            return this.Moban.ShengchengLiuchengtu(this);
        }

        internal void Jiazai()
        {
            List<XingdongModel> modellList = NHibernateHelper.CurrentSession.QueryOver<XingdongModel>().Where(x => x.LiuchengId == this.Id).List().ToList();
            foreach (XingdongModel model in modellList)
            {
                Xingdong renwu = this.ChuangjianXingdong(model);
                renwu.Jiazai();
            }
        }

        public LiuchengXinxi Map()
        {
            return new LiuchengXinxi 
            { 
                Faqiren = this.Faqiren.MapUserInfo(),
                FaqiShijian = this.FaqiShijian,
                Id = this.Id,
                JieshuShijian = this.JieshuShijian,
                Liucheng = this.Moban.Map(),
                Mingcheng = this.Mingcheng,
                Zhuangtai = this.Zhuangtai,
                Guid = this.Guid,
                Zhaiyao = this.Zhaiyao
            };
        }
    }
}
