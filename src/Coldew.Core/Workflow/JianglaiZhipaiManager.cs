using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core.Organization;
using System.Threading;
using Coldew.Api.Workflow.Exceptions;
using Coldew.Data;

namespace Coldew.Core.Workflow
{
    public class JianglaiZhipaiManager
    {
        private Dictionary<User, JianglaiRenwuZhipai> _jianglaiZhipaiDicByUser;
        OrganizationManagement _orgManager;
        ReaderWriterLock _lock;

        public JianglaiZhipaiManager(OrganizationManagement orgManager)
        {
            this._jianglaiZhipaiDicByUser = new Dictionary<User, JianglaiRenwuZhipai>();
            this._orgManager = orgManager;
            this._lock = new ReaderWriterLock();
        }

        public JianglaiRenwuZhipai GetJaingLaiZhipai(User zhipairen)
        {   
            this._lock.AcquireReaderLock(0);
            try
            {
                if (this._jianglaiZhipaiDicByUser.ContainsKey(zhipairen))
                {
                    return this._jianglaiZhipaiDicByUser[zhipairen];
                }
                return null;
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        public void SetJianglaiRenwuZhipai(User zhipairen, User dailiren, DateTime? kaishiShijian, DateTime? jieshuShijian)
        {
            this._lock.AcquireWriterLock(0);
            try
            {
                if (zhipairen == dailiren)
                {
                    throw new JianglaiZhipaiGeiZijiException();
                }

                if (!kaishiShijian.HasValue && !jieshuShijian.HasValue)
                {
                    kaishiShijian = DateTime.Now;
                }

                if (this._jianglaiZhipaiDicByUser.ContainsKey(zhipairen))
                {
                    JianglaiRenwuZhipai jianglaiRenwuZhipai = this._jianglaiZhipaiDicByUser[zhipairen];
                    jianglaiRenwuZhipai.Xiugai(dailiren, kaishiShijian, jieshuShijian);
                }
                else
                {
                    JianglaiRenwuZhipaiModel model = new JianglaiRenwuZhipaiModel();
                    model.Dailiren = dailiren.Account;
                    model.JieshuShijian = jieshuShijian;
                    model.KaishiShijian = kaishiShijian;
                    model.Zhipairen = zhipairen.Account;
                    model.Id = (int)NHibernateHelper.CurrentSession.Save(model);
                    this.ChuangjianJianglaiRenwuZhipai(model);
                }
            }
            finally
            {
                this._lock.ReleaseWriterLock();
            }
        }

        public void QuxiaoJianglaiRenwuZhipai(User zhipairen)
        {
            this._lock.AcquireWriterLock(0);
            try
            {
                if (this._jianglaiZhipaiDicByUser.ContainsKey(zhipairen))
                {
                    JianglaiRenwuZhipaiModel model = NHibernateHelper.CurrentSession.Get<JianglaiRenwuZhipaiModel>(this._jianglaiZhipaiDicByUser[zhipairen].Id);
                    NHibernateHelper.CurrentSession.Delete(model);
                    NHibernateHelper.CurrentSession.Flush();
                    this._jianglaiZhipaiDicByUser.Remove(zhipairen);
                }
            }
            finally
            {
                this._lock.ReleaseWriterLock();
            }
        }

        internal JianglaiRenwuZhipai ChuangjianJianglaiRenwuZhipai(JianglaiRenwuZhipaiModel model)
        {
            User zhipairen = this._orgManager.UserManager.GetUserByAccount(model.Zhipairen);
            User dailiren = this._orgManager.UserManager.GetUserByAccount(model.Dailiren);
            JianglaiRenwuZhipai zhipai = new JianglaiRenwuZhipai(model.Id, zhipairen, dailiren, model.KaishiShijian, model.JieshuShijian);
            this._jianglaiZhipaiDicByUser.Add(zhipairen, zhipai);
            return zhipai;
        }

        internal void Jiazai()
        {
            IList<JianglaiRenwuZhipaiModel> jianglaiModels = NHibernateHelper.CurrentSession.QueryOver<JianglaiRenwuZhipaiModel>().List();
            foreach (JianglaiRenwuZhipaiModel model in jianglaiModels)
            {
                this.ChuangjianJianglaiRenwuZhipai(model);
            }
        }
    }
}
