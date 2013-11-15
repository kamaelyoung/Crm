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
        public LiuchengYinqing(ColdewManager coldewManager)
        {
            this.Logger = coldewManager.Logger;
            this.ColdewManager = coldewManager;
            this.LiuchengMobanManager = new LiuchengMobanManager(this, this.ColdewManager.ObjectManager);
            this.JianglaiZhipaiManager = new Workflow.JianglaiZhipaiManager(this.ColdewManager.OrgManager);
            this.ZhipaiManager = new Workflow.ZhipaiManager(this.ColdewManager.OrgManager);
            this.LiuchengManager = new LiuchengManager(this);
            this.RenwuNotifyManager= new RenwuNotifyManager(this);
        }
        public ColdewManager ColdewManager { private set; get; }

        public LiuchengMobanManager LiuchengMobanManager { private set; get; }

        public LiuchengManager LiuchengManager { private set; get; }

        public RenwuNotifyManager RenwuNotifyManager { private set; get; }

        public ZhipaiManager ZhipaiManager { private set; get; }

        public JianglaiZhipaiManager JianglaiZhipaiManager { private set; get; }

        public User GetYonghu(string zhanghao)
        {
            return this.ColdewManager.OrgManager.UserManager.GetUserByAccount(zhanghao);
        }

        public User GetYonghuByGuid(string guid)
        {
            return this.ColdewManager.OrgManager.UserManager.GetUserById(guid);
        }

        public ILog Logger { private set; get; }

        internal void Load()
        {
            this.LiuchengMobanManager.Load();
            this.LiuchengManager.Jiazai();
            this.RenwuNotifyManager.Load();
        }
    }
}
