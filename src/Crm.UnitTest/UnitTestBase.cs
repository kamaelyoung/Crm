using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using log4net;
using System.IO;
using Crm.Core.Organization;
using Crm.Api.Organization;
using Crm.Core;

namespace Crm.UnitTest
{
    public class UnitTestBase
    {
        public UnitTestBase()
        {
            string log4ConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4.config"); ;
            log4net.Config.XmlConfigurator.Configure(new FileInfo(log4ConfigPath));
            this.Logger = log4net.LogManager.GetLogger("TestLogger");

            this.CrmManager = new CrmManager();

            this.Org = this.CrmManager.OrgManager;
            this.Org.Logger = this.Logger;
            new OrganizationInitializer(this.Org);
            new CrmInitializer(this.CrmManager);
            this.Org.PositionManager.Create(this.Org.System, new PositionCreateInfo { Name = "position1", ParentId = this.Org.PositionManager.TopPosition.ID });
            User1 = this.Org.UserManager.Create(this.Org.System, new UserCreateInfo { Account = "user1", Password = "edoc2", Name = "user1", MainPositionId = this.Org.PositionManager.TopPosition.ID });
            User2 = this.Org.UserManager.Create(this.Org.System, new UserCreateInfo { Account = "user2", Password = "edoc2", Name = "user2", MainPositionId = this.Org.PositionManager.TopPosition.ID });

            this.Admin = this.Org.UserManager.GetUserByAccount("admin");

            this.CustomerManager = this.CrmManager.CustomerManager;

        }

        protected OrganizationManagement Org { set; get; }

        protected CrmManager CrmManager { set; get; }

        protected CustomerManager CustomerManager { set; get; }

        protected User Admin { set; get; }
        protected User User1 { set; get; }
        protected User User2 { set; get; }

        public ILog Logger { private set; get; }

        [SetUp]
        public void Setup()
        {
            
        }
    }
}
