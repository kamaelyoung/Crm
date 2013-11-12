using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api.Workflow;
using Coldew.Data;
using Coldew.Api.Workflow.Exceptions;
using Coldew.Core.Organization;

namespace Coldew.Core.Workflow
{
    public class Renwu
    {
        LiuchengYinqing _yingqing;

        public Renwu(int id, string guid, User yongyouren, User chuliren, User shijiChuliren, 
            DateTime? chuliShijian, RenwuZhuangtai zhuangtai, RenwuChuliJieguo? chuliJieguo, string chuliShuoming, Xingdong xingdong)
        {
            this.Id = id;
            this.Guid = guid;
            this.Chuliren = chuliren;
            this.ShijiChuliren = shijiChuliren;
            this.Zhuangtai = zhuangtai;
            this.ChuliShijian = chuliShijian;
            this.ChuliShuoming = chuliShuoming;
            this.Yongyouren = yongyouren;
            this.ChuliJieguo = chuliJieguo;
            this._yingqing = xingdong.Yingqing;
            this.Xingdong = xingdong;
            if (this.Chuliren != this.Yongyouren && this.Zhuangtai == RenwuZhuangtai.Chulizhong)
            {

                this._yingqing.ZhipaiManager.TianjiaZhipaideRenwu(this.Yongyouren, this.Chuliren, this);
            }
            this.Xingdong.Wanchenghou += new TEventHanlder<Workflow.Xingdong>(Xingdong_Wanchenghou);
        }

        void Xingdong_Wanchenghou(Xingdong args)
        {
            if (this.Zhuangtai == RenwuZhuangtai.Chulizhong)
            {
                this.Shanchu();
            }
        }

        public Xingdong Xingdong { private set; get; }

        public int Id { private set; get; }

        public string Guid { private set; get; }

        public User Yongyouren { private set; get; }

        public User Chuliren { private set; get; }

        public User ShijiChuliren { private set; get; }

        public RenwuZhuangtai Zhuangtai { private set; get; }

        public DateTime? ChuliShijian { private set; get; }

        public string ChuliShuoming { private set; get; }

        public RenwuChuliJieguo? ChuliJieguo { private set; get; }

        public event TEventHanlder<Renwu> Wanchenghou;

        private object _lock = new object();

        public virtual void Wancheng(User chuliren, string shuoming)
        {
            lock (_lock)
            {
                if (this.Zhuangtai == RenwuZhuangtai.Wanchengle)
                {
                    throw new RenwuChongfuChuliException(this.ShijiChuliren.Name, this.ChuliShijian.Value);
                }
                RenwuModel model = NHibernateHelper.CurrentSession.Get<RenwuModel>(this.Id);
                model.ChuliShijian = DateTime.Now;
                model.ShijiChuliren = chuliren.Account;
                model.ChuliShuoming = shuoming;
                model.Zhuangtai = (int)RenwuZhuangtai.Wanchengle;
                NHibernateHelper.CurrentSession.Update(model);
                NHibernateHelper.CurrentSession.Flush();

                this.ChuliShijian = DateTime.Now;
                this.ShijiChuliren = chuliren;
                this.ChuliShuoming = shuoming;
                this.Zhuangtai = RenwuZhuangtai.Wanchengle;

                if (this.Wanchenghou != null)
                {
                    this.Wanchenghou(this);
                }

                this._yingqing.ZhipaiManager.YichuZhipaideRenwu(this.Yongyouren, this);
            }
        }

        public event TEventHanlder<Renwu> Shanchuhou;

        public virtual void Shanchu()
        {
            lock (_lock)
            {
                RenwuModel model = NHibernateHelper.CurrentSession.Get<RenwuModel>(this.Id);
                NHibernateHelper.CurrentSession.Delete(model);
                NHibernateHelper.CurrentSession.Flush();

                if (this.Shanchuhou != null)
                {
                    this.Shanchuhou(this);
                }
            }
        }

        public void Zhipai(User chuliren)
        {
            if (chuliren == this.Yongyouren)
            {
                throw new RenwuZhipaiGeiZijiException();
            }
            if (this.Yongyouren != this.Chuliren)
            {
                throw new RenwuChongfuZhipaiException();
            }
            RenwuModel model = NHibernateHelper.CurrentSession.Get<RenwuModel>(this.Id);
            model.Chuliren = chuliren.Account;
            NHibernateHelper.CurrentSession.Update(model);
            NHibernateHelper.CurrentSession.Flush();

            this.Chuliren = chuliren;
            
            this._yingqing.ZhipaiManager.TianjiaZhipaideRenwu(this.Yongyouren, this.Chuliren, this);
        }

        public void XiugaiChuliren(User chuliren)
        {
            RenwuModel model = NHibernateHelper.CurrentSession.Get<RenwuModel>(this.Id);
            model.Yongyouren = chuliren.Account;
            model.Chuliren = chuliren.Account;
            NHibernateHelper.CurrentSession.Update(model);
            NHibernateHelper.CurrentSession.Flush();

            this.Chuliren = chuliren;
            this.Yongyouren = chuliren;
            this._yingqing.ZhipaiManager.YichuZhipaideRenwu(this.Yongyouren, this);
        }

        public void QuxiaoZhipai()
        {
            RenwuModel model = NHibernateHelper.CurrentSession.Get<RenwuModel>(this.Id);
            model.Chuliren = this.Yongyouren.Account;
            NHibernateHelper.CurrentSession.Update(model);
            NHibernateHelper.CurrentSession.Flush();

            this.Chuliren = this.Yongyouren;
            this._yingqing.ZhipaiManager.YichuZhipaideRenwu(this.Yongyouren, this);
        }

        public bool NengChuli(User chuliren)
        {
            if(this.Chuliren.Equals(chuliren))
            {
                return true;
            }
            return false;
        }

        public RenwuXinxi Map()
        {
            return new RenwuXinxi
            {
                Id = this.Id,
                Guid = this.Guid,
                ChuliShijian = this.ChuliShijian,
                ChuliShuoming = this.ChuliShuoming,
                Chuliren = Chuliren.MapUserInfo(),
                Xingdong = this.Xingdong.Map(),
                Zhuangtai = this.Zhuangtai,
                Bianhao = this.Xingdong.Code
            };
        }
    }
}
