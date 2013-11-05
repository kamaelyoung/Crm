using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Data;
using Coldew.Api.Workflow;
using NHibernate.Criterion;
using log4net;
using System.IO;
using System.Drawing;
using System.Collections;
using Coldew.Api.Workflow.Exceptions;
using Coldew.Core.Organization;

namespace Coldew.Core.Workflow
{
    public class LiuchengYinqing
    {
        ColdewManager _coldewManager;

        public LiuchengYinqing(ColdewManager coldewManager)
        {
            this.Logger = coldewManager.Logger;
            this._coldewManager = coldewManager;
            this.LiuchengMobanManager = new LiuchengMobanManager(this, this._coldewManager.ObjectManager);
            this.JianglaiZhipaiManager = new Workflow.JianglaiZhipaiManager(this._coldewManager.OrgManager);
            this.ZhipaiManager = new Workflow.ZhipaiManager(this._coldewManager.OrgManager);

        }

        public LiuchengMobanManager LiuchengMobanManager { private set; get; }

        public ZhipaiManager ZhipaiManager { private set; get; }

        public JianglaiZhipaiManager JianglaiZhipaiManager { private set; get; }

        public User GetYonghu(string zhanghao)
        {
            return this._coldewManager.OrgManager.UserManager.GetUserByAccount(zhanghao);
        }

        public User GetYonghuByGuid(string guid)
        {
            return this._coldewManager.OrgManager.UserManager.GetUserById(guid);
        }

        public ILog Logger { private set; get; }
    }
}
