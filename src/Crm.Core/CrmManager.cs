using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Data;
using Crm.Core.Organization;
using log4net;
using Crm.Core.Extend;

namespace Crm.Core
{
    public class CrmManager
    {
        public CrmManager()
        {
            log4net.Config.XmlConfigurator.Configure();
            this.Logger = log4net.LogManager.GetLogger("logger");

            this.OrgManager = new OrganizationManagement();
            this.AreaManager = new CustomerAreaManager(this.OrgManager);
            this.FormManager = new FormManager();
            this.GridViewManager = new GridViewManager(this.OrgManager, this.FormManager);
            this.CustomerManager = new CustomerManager(this.AreaManager, this.OrgManager, this.FormManager);
            this.ContactManager = new ContactManager(this.CustomerManager, this.OrgManager, this.FormManager);
            this.ActivityManager = new ActivityManager(this.ContactManager, this.OrgManager, this.FormManager);
            this.ContractManager = new ContractManager(this.CustomerManager, this.OrgManager, this.FormManager);
            this.FavoriteManager = new CustomerFavoriteManager(this.CustomerManager, this.OrgManager);
            this.ConfigManager = new CrmConfigManager(this);
            this.ContractEmailNotifyService = new ContractEmailNotifyService(this);
            this.ContractEmailNotifyService.Start();
        }

        public CustomerManager CustomerManager { set; get; }

        public ContactManager ContactManager { set; get; }

        public ContractManager ContractManager { set; get; }

        public ActivityManager ActivityManager { set; get; }

        public CustomerFavoriteManager FavoriteManager { set; get; }

        public CustomerAreaManager AreaManager { set; get; }

        public FormManager FormManager { set; get; }

        public GridViewManager GridViewManager { set; get; }

        public OrganizationManagement OrgManager { set; get; }

        public CrmConfigManager ConfigManager { set; get; }

        public MailSender MailSender { set; get; }

        public ContractEmailNotifyService ContractEmailNotifyService { set; get; }

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
