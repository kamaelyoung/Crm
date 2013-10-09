using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Coldew.Core.Organization;

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
            this.FormManager = this.CreateFormManager();
            this.GridViewManager = new GridViewManager(this.OrgManager, this.FormManager);
            this.ConfigManager = new CrmConfigManager(this);
        }

        protected virtual void Load()
        {
            this.FormManager.Load();
            this.GridViewManager.Load();
            this.ConfigManager.Load();
        }

        protected virtual FormManager CreateFormManager()
        {
            return new FormManager(this);
        }

        public OrganizationManagement OrgManager { set; get; }

        public FormManager FormManager { set; get; }

        public GridViewManager GridViewManager { set; get; }

        public CrmConfigManager ConfigManager { set; get; }

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
