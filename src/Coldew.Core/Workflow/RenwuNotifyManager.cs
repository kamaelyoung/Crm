using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Coldew.Data;

namespace Coldew.Core.Workflow
{
    public class RenwuNotifyManager
    {
        protected Dictionary<string, List<RenwuNotify>> _renwuNotifyDicByRenwuId;
        protected ReaderWriterLock _lock;
        RenwuEmailNotifyService _notifyService;

        public RenwuNotifyManager(LiuchengYinqing yinqing)
        {
            this._renwuNotifyDicByRenwuId = new Dictionary<string, List<RenwuNotify>>();
            this._notifyService = new RenwuEmailNotifyService(yinqing.ColdewManager);
            this._lock = new ReaderWriterLock();
        }

        public RenwuNotify Create(string renwuId, string userAccount, string subject, string body)
        {
            this._lock.AcquireWriterLock(0);
            try
            {
                RenwuNotifyModel model = new RenwuNotifyModel();
                model.RenwuId = renwuId;
                model.UserAccount = userAccount;
                model.NotifyTime = DateTime.Now;
                model.Subject = subject;
                model.Body = body;

                model.ID = NHibernateHelper.CurrentSession.Save(model).ToString();
                NHibernateHelper.CurrentSession.Flush();

                return this.Create(model);
            }
            finally
            {
                this._lock.ReleaseWriterLock();
            }
        }

        private RenwuNotify Create(RenwuNotifyModel model)
        {
            RenwuNotify renwuNotify = new RenwuNotify(model.ID, model.RenwuId, model.UserAccount, model.NotifyTime, model.Subject, model.Body);

            if (!this._renwuNotifyDicByRenwuId.ContainsKey(model.RenwuId))
            {
                this._renwuNotifyDicByRenwuId.Add(model.RenwuId, new List<RenwuNotify>());
            }
            this._renwuNotifyDicByRenwuId[model.RenwuId].Add(renwuNotify);
            return renwuNotify;
        }

        public List<RenwuNotify> GetRenwuNotifys(string renwuId)
        {
            if (this._renwuNotifyDicByRenwuId.ContainsKey(renwuId))
            {
                return this._renwuNotifyDicByRenwuId[renwuId];
            }
            return new List<RenwuNotify>();
        }

        internal void Load()
        {
            IList<RenwuNotifyModel> models = NHibernateHelper.CurrentSession.QueryOver<RenwuNotifyModel>().List();
            foreach (RenwuNotifyModel model in models)
            {
                this.Create(model);
            }
            this._notifyService.Start();
        }
    }
}
