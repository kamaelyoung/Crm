using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using log4net;
using System.IO;
using Coldew.Core.Organization;
using Coldew.Api.Organization;
using Coldew.Core;

namespace Coldew.NnitTest
{
    public class UnitTestBase
    {
        public UnitTestBase()
        {
            string log4ConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4.config"); ;
            log4net.Config.XmlConfigurator.Configure(new FileInfo(log4ConfigPath));
            this.Logger = log4net.LogManager.GetLogger("TestLogger");

            this.ColdewManager = new ColdewManager();

            this.Org = this.ColdewManager.OrgManager;
            this.Org.Logger = this.Logger;
            new OrganizationInitializer(this.Org);
            this.Org.PositionManager.Create(this.Org.System, new PositionCreateInfo { Name = "position1", ParentId = this.Org.PositionManager.TopPosition.ID });
            User1 = this.Org.UserManager.Create(this.Org.System, new UserCreateInfo { Account = "user1", Password = "edoc2", Name = "user1", MainPositionId = this.Org.PositionManager.TopPosition.ID });
            User2 = this.Org.UserManager.Create(this.Org.System, new UserCreateInfo { Account = "user2", Password = "edoc2", Name = "user2", MainPositionId = this.Org.PositionManager.TopPosition.ID });
            User3 = this.Org.UserManager.Create(this.Org.System, new UserCreateInfo { Account = "user3", Password = "edoc2", Name = "user3", MainPositionId = this.Org.PositionManager.TopPosition.ID });
            User4 = this.Org.UserManager.Create(this.Org.System, new UserCreateInfo { Account = "user4", Password = "edoc2", Name = "user4", MainPositionId = this.Org.PositionManager.TopPosition.ID });
            User5 = this.Org.UserManager.Create(this.Org.System, new UserCreateInfo { Account = "user5", Password = "edoc2", Name = "user5", MainPositionId = this.Org.PositionManager.TopPosition.ID });

            this.Admin = this.Org.UserManager.GetUserByAccount("admin");


        }

        protected OrganizationManagement Org { set; get; }

        protected ColdewManager ColdewManager { set; get; }

        protected User Admin { set; get; }
        protected User User1 { set; get; }
        protected User User2 { set; get; }
        protected User User3 { set; get; }
        protected User User4 { set; get; }
        protected User User5 { set; get; }

        public ILog Logger { private set; get; }

        [SetUp]
        public void Setup()
        {
            
        }
    }
}
