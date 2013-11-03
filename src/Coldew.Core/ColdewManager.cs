using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Coldew.Core.Organization;
using Coldew.Core.Workflow;

namespace Coldew.Core
{
    public class ColdewManager
    {
        public ColdewManager()
        {
            log4net.Config.XmlConfigurator.Configure();
            this.Logger = log4net.LogManager.GetLogger("logger");

            this.Init();
            this.Load();
        }

        protected virtual void Init()
        {
            this.OrgManager = new OrganizationManagement();
            this.ObjectManager = this.CreateFormManager();
            this.ConfigManager = new ColdewConfigManager(this);
            this.LiuchengYinqing = new LiuchengYinqing(this);
        }

        protected virtual void Load()
        {
            this.ObjectManager.Load();
            this.ConfigManager.Load();
        }

        protected virtual ColdewObjectManager CreateFormManager()
        {
            return new ColdewObjectManager(this);
        }

        public LiuchengYinqing LiuchengYinqing { set; get; }

        public OrganizationManagement OrgManager { set; get; }

        public ColdewObjectManager ObjectManager { set; get; }

        public ColdewConfigManager ConfigManager { set; get; }

        public MailSender MailSender { set; get; }

        ILog _logger;
        public ILog Logger
        {
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                _logger = value;
            }
            get
            {
                return _logger;
            }
        }
    }
}
