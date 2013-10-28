using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Data;
using Coldew.Core.Organization;
using Coldew.Api.Workflow.Exceptions;
using System.Threading;

namespace Coldew.Core.Workflow
{
    public class ZhipaiManager
    {
        private Dictionary<User, List<Zhipai>> _zhipaiDicByUser;
        private Dictionary<User, JianglaiRenwuZhipai> _jianglaiZhipaiDicByUser;
        OrganizationManagement _orgManager;
        ReaderWriterLock _lock;

        public ZhipaiManager(OrganizationManagement orgManager)
        {
            this._zhipaiDicByUser = new Dictionary<User, List<Zhipai>>();
            this._jianglaiZhipaiDicByUser = new Dictionary<User, JianglaiRenwuZhipai>();
            this._orgManager = orgManager;
            this._lock = new ReaderWriterLock();
        }

        public List<Renwu> GetZhipaideRenwuList(User zhipairen)
        {
            this._lock.AcquireReaderLock(0);
            try
            {
                if (this._zhipaiDicByUser.ContainsKey(zhipairen))
                {
                    return this._zhipaiDicByUser[zhipairen].Select(x => x.Renwu).ToList();
                }
                return new List<Renwu>();
            }
            finally
            {
                this._lock.ReleaseReaderLock();
            }
        }

        internal void TianjiaZhipaideRenwu(User zhipairen, User dailiren, Renwu renwu)
        {
            this._lock.AcquireWriterLock(0);
            try
            {
                if (!this._zhipaiDicByUser.ContainsKey(zhipairen))
                {
                    this._zhipaiDicByUser.Add(zhipairen, new List<Zhipai>());
                }
                Zhipai zhipai = new Zhipai(zhipairen, dailiren, renwu);
                this._zhipaiDicByUser[zhipairen].Add(zhipai);
            }
            finally
            {
                this._lock.ReleaseWriterLock();
            }
        }

        internal void YichuZhipaideRenwu(User zhipairen, Renwu renwu)
        {
            this._lock.AcquireWriterLock(0);
            try
            {
                if (this._zhipaiDicByUser.ContainsKey(zhipairen))
                {
                    Zhipai zhipai = this._zhipaiDicByUser[zhipairen].Find(x => x.Renwu == renwu);
                    if (zhipai != null)
                    {
                        this._zhipaiDicByUser[zhipairen].Remove(zhipai);
                    }
                }
            }
            finally
            {
                this._lock.ReleaseWriterLock();
            }
        }
    }
}
