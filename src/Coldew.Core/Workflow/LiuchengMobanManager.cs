using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Coldew.Data;
using Newtonsoft.Json;
using Coldew.Api.Workflow;

namespace Coldew.Core.Workflow
{
    public class LiuchengMobanManager
    {
        ReaderWriterLock _lock;
        List<LiuchengMoban> _mobanList;
        ColdewObjectManager _objectManager;
        LiuchengYinqing _yinqing;

        public LiuchengMobanManager(LiuchengYinqing yinqing, ColdewObjectManager objectManager)
        {
            this._lock = new ReaderWriterLock();
            this._mobanList = new List<LiuchengMoban>();
            this._objectManager = objectManager;
            this._yinqing = yinqing;

            this.Load();
        }

        public LiuchengMoban Create(string code, string name, ColdewObject cobject, string transferUrl, string remark)
        {
            this._lock.AcquireWriterLock(0);
            try
            {

                LiuchengMobanModel model = new LiuchengMobanModel();
                model.Code = code;
                model.Name = name;
                model.ColdewObjectCode = cobject.Code;
                model.Remark = remark;
                model.TransferUrl = transferUrl;
                NHibernateHelper.CurrentSession.Save(model);
                NHibernateHelper.CurrentSession.Flush();

                return this.Create(model);
            }
            finally
            {
                this._lock.ReleaseWriterLock();
            }
        }

        public LiuchengMoban GetMobanById(string id)
        {
            this._lock.AcquireReaderLock(0);
            try
            {
                return this._mobanList.Find(x => x.ID == id);
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public LiuchengMoban GetMobanByCode(string code)
        {
            this._lock.AcquireReaderLock(0);
            try
            {
                return this._mobanList.Find(x => x.Code == code);
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public List<LiuchengMoban> GetAllMoban()
        {
            this._lock.AcquireReaderLock(0);
            try
            {
                return this._mobanList.ToList();
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        private LiuchengMoban Create(LiuchengMobanModel model)
        {
            ColdewObject cobject = this._objectManager.GetFormByCode(model.ColdewObjectCode);

            LiuchengMoban moban = new LiuchengMoban(model.ID, model.Code, model.Name, model.TransferUrl, model.Remark, this._yinqing, cobject);
            this._mobanList.Add(moban);
            return moban;
        }

        public Renwu GetRenwu(int renwuId)
        {
            foreach (LiuchengMoban liuchengMoban in this._mobanList)
            {
                foreach (Liucheng liucheng in liuchengMoban.LiuchengList)
                {
                    foreach (Xingdong renwu in liucheng.XingdongList)
                    {
                        Renwu xingdong = renwu.RenwuList.Find(x => x.Id == renwuId);
                        if (xingdong != null)
                        {
                            return xingdong;
                        }
                    }
                }
            }
            return null;
        }

        public Renwu GetRenwu(string renwuId)
        {
            foreach (LiuchengMoban liuchengMoban in this._mobanList)
            {
                foreach (Liucheng liucheng in liuchengMoban.LiuchengList)
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
            }
            return null;
        }

        public Liucheng GetLiucheng(string id)
        {
            foreach (LiuchengMoban liuchengMoban in this._mobanList)
            {
                Liucheng liucheng = liuchengMoban.GetLiucheng(id);
                if (liucheng != null)
                {
                    return liucheng;
                }
            }
            return null;
        }

        public Xingdong GetXingdong(string guid)
        {
            foreach (LiuchengMoban liuchengMoban in this._mobanList)
            {
                foreach (Liucheng liucheng in liuchengMoban.LiuchengList)
                {
                    Xingdong renwu = liucheng.GetXingdong(guid);
                    if (renwu != null)
                    {
                        return renwu;
                    }
                }
            }
            return null;
        }

        private void Load()
        {
            IList<LiuchengMobanModel> models = NHibernateHelper.CurrentSession.QueryOver<LiuchengMobanModel>().List();
            foreach (LiuchengMobanModel model in models)
            {
                this.Create(model);
            }
        }
    }
}
