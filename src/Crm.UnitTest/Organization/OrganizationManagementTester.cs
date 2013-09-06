using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Newtonsoft.Json.Linq;
using System.Threading;
using Crm.Core.Organization;
using Crm.Api.Organization;
using Crm.Api.Organization.Exceptions;

namespace Crm.UnitTest.Management
{
    [TestFixture]
    public class OrganizationManagementTester : UnitTestBase
    {
        [Test]
        public void UserManagerTest()
        {
            User testUser1 = this.Org.UserManager.Create(this.Admin, new UserCreateInfo { Account = Guid.NewGuid().ToString(),  Password = "edoc2", Name = Guid.NewGuid().ToString(), MainPositionId = this.Org.PositionManager.TopPosition.ID });

            //CheckPassword
            Assert.IsTrue(this.Org.UserManager.CheckPassword(testUser1.Account, "edoc2"));

            //GetUser
            Assert.AreEqual(testUser1, this.Org.UserManager.GetUserByAccount(testUser1.Account));
            Assert.AreEqual(testUser1, this.Org.UserManager.GetUserById(testUser1.ID));
            Assert.AreEqual(testUser1, this.Org.UserManager.Search(testUser1.Account, testUser1.Name)[0]);
            Assert.AreEqual(testUser1, this.Org.UserManager.Search(testUser1.Account)[0]);


            this.Org.UserManager.Delete(this.Admin, testUser1.ID);
            Assert.AreEqual(null, this.Org.UserManager.GetUserById(testUser1.ID));
        }

        [Test]
        public void UserTest()
        {
            User testUser1 = this.Org.UserManager.Create(this.Admin, new UserCreateInfo { Account = Guid.NewGuid().ToString(), Password = "edoc2", Name = Guid.NewGuid().ToString(), MainPositionId = this.Org.PositionManager.TopPosition.ID });
            UserChangeInfo changeInfo = new UserChangeInfo(testUser1.MapUserInfo());
            
            //Change
            testUser1.Change(this.Admin, changeInfo);

            //ChangeSignInInfo
            UserSignInChangeInfo signInInfo = new UserSignInChangeInfo(testUser1.MapUserInfo());
            signInInfo.LastLoginIp = "127.0.0.1";
            testUser1.ChangeSignInInfo(this.Admin, signInInfo);
            Assert.AreEqual("127.0.0.1", testUser1.LastLoginIp);

            //ChangePassword
            testUser1.ChangePassword("edoc2", "newedoc2");
            Assert.AreEqual(Cryptography.MD5Encode("newedoc2"), testUser1.Password);

            testUser1.Lock(this.Admin);
            Assert.AreEqual(UserStatus.Lock, testUser1.Status);
            testUser1.Activate(this.Admin);
            Assert.AreEqual(UserStatus.Normal, testUser1.Status);
            testUser1.Logoff(this.Admin);
            Assert.AreEqual(UserStatus.Logoff, testUser1.Status);
            testUser1.Activate(this.Admin);

            //relation
            Position position = this.Org.PositionManager.Create(this.Admin,
                new PositionCreateInfo { Name = Guid.NewGuid().ToString(), ParentId = this.Org.PositionManager.TopPosition.ID });
            User positionUser1 = this.Org.UserManager.Create(this.Admin,
                new UserCreateInfo { Account = Guid.NewGuid().ToString(), Password = "edoc2", Name = Guid.NewGuid().ToString(), MainPositionId = position.ID });

            Department department = this.Org.DepartmentManager.Create(this.Admin,
                new DepartmentCreateInfo { Name = Guid.NewGuid().ToString(), ManagerPositionInfo = new PositionCreateInfo { Name = Guid.NewGuid().ToString(), ParentId = this.Org.PositionManager.TopPosition.ID } });
            User departmentManagerUser1 = this.Org.UserManager.Create(this.Admin,
                new UserCreateInfo { Account = Guid.NewGuid().ToString(), Password = "edoc2", Name = Guid.NewGuid().ToString(), MainPositionId = department.ManagerPosition.ID });

            Position position1 = this.Org.PositionManager.Create(this.Admin,
                new PositionCreateInfo { Name = Guid.NewGuid().ToString(), ParentId = department.ManagerPosition.ID });
            User position1User1 = this.Org.UserManager.Create(this.Admin, new UserCreateInfo { Account = Guid.NewGuid().ToString(), Password = "edoc2", Name = Guid.NewGuid().ToString(), MainPositionId = position1.ID });


            Assert.IsTrue(positionUser1.IsMySuperior(testUser1, false));
            Assert.IsTrue(departmentManagerUser1.IsMySuperior(testUser1, false));
            Assert.IsTrue(position1User1.IsMySuperior(testUser1, true));
            Assert.IsFalse(position1User1.IsMySuperior(testUser1, false));
            Assert.IsFalse(departmentManagerUser1.IsMySuperior(positionUser1, false));
            Assert.IsTrue(positionUser1.IsMyDepartmentManagers(testUser1));
        }

        [Test]
        public void PositionManagerTest()
        {
            Position testPositoin1 = this.Org.PositionManager.Create(this.Admin, new PositionCreateInfo { Name = Guid.NewGuid().ToString(), ParentId = this.Org.PositionManager.TopPosition.ID});

            //Get
            Assert.AreEqual(testPositoin1, this.Org.PositionManager.GetPositionById(testPositoin1.ID));
        }

        [Test]
        public void PositionTest()
        {
            Position testPositoin1 = this.Org.PositionManager.Create(this.Admin, new PositionCreateInfo { Name = Guid.NewGuid().ToString(), ParentId = this.Org.PositionManager.TopPosition.ID });

            PositionChangeInfo changeInfo = new PositionChangeInfo(testPositoin1.MapPositionInfo());
            testPositoin1.Change(this.Admin, changeInfo);

            //relation
            User testUser2 = this.Org.UserManager.Create(this.Admin, new UserCreateInfo { Account = Guid.NewGuid().ToString(), Password = "edoc2", Name = Guid.NewGuid().ToString(), MainPositionId = testPositoin1.ID });
            Assert.AreEqual(testUser2, testPositoin1.Users[0]);
            Assert.IsTrue(testPositoin1.InPosition(testUser2, false));

            Position testPositoin2 = this.Org.PositionManager.Create(this.Admin, new PositionCreateInfo { Name = Guid.NewGuid().ToString(), ParentId = testPositoin1.ID });
            Assert.IsTrue(testPositoin2.IsMySuperior(testPositoin1, true));
        }

        [Test]
        public void DepartmentManagerTest()
        {
            Department testDept1 = this.Org.DepartmentManager.Create(this.Admin, new DepartmentCreateInfo { Name = Guid.NewGuid().ToString(), ManagerPositionInfo = new PositionCreateInfo { Name = Guid.NewGuid().ToString(), ParentId = this.Org.PositionManager.TopPosition.ID } });

            //Get
            Assert.AreEqual(testDept1, this.Org.DepartmentManager.GetDepartmentById(testDept1.ID));

            this.Org.DepartmentManager.Delete(this.Admin, testDept1.ID);
        }

        [Test]
        public void DepartmentTest()
        {
            Department testDept1 = this.Org.DepartmentManager.Create(this.Admin, new DepartmentCreateInfo { Name = Guid.NewGuid().ToString(), ManagerPositionInfo = new PositionCreateInfo { Name = Guid.NewGuid().ToString(), ParentId = this.Org.PositionManager.TopPosition.ID } });

            //change
            DepartmentChangeInfo changeInfo = new DepartmentChangeInfo(testDept1.MapDepartmentInfo());
            changeInfo.Remark = "r1";
            testDept1.Change(this.Admin, changeInfo);
            Assert.AreEqual("r1", changeInfo.Remark);

            //relation
            User user1 = this.Org.UserManager.Create(this.Admin, new UserCreateInfo { Account = Guid.NewGuid().ToString(), Password = "edoc2", Name = Guid.NewGuid().ToString(), MainPositionId = testDept1.ManagerPosition.ID });            
            Position position1 = this.Org.PositionManager.Create(this.Admin, new PositionCreateInfo{ Name = Guid.NewGuid().ToString(), ParentId = testDept1.ManagerPosition.ID });
            User user2 = this.Org.UserManager.Create(this.Admin, new UserCreateInfo { Account = Guid.NewGuid().ToString(), Password = "edoc2", Name = Guid.NewGuid().ToString(), MainPositionId = position1.ID });

            Assert.AreEqual(2, testDept1.AllUsers.Count);
            Assert.IsTrue(testDept1.InDepartment(user1, false));
            Assert.IsTrue(testDept1.InDepartment(user2, true));
            Assert.AreEqual(2, testDept1.Positions.Count);
            Assert.AreEqual(this.Org.PositionManager.TopPosition, testDept1.PositionParent);
        }

        [Test]
        public void GroupManagerTest()
        {
            Group group1 = this.Org.GroupManager.Create(this.Admin, new GroupCreateInfo { Name = Guid.NewGuid().ToString() });
            Group personalGroup1 = this.Org.GroupManager.Create(this.Admin, new GroupCreateInfo { Name = Guid.NewGuid().ToString(), GroupType = GroupType.Personal });
            Group personalGroup2 = this.Org.GroupManager.Create(this.Admin, new GroupCreateInfo { Name = Guid.NewGuid().ToString(), GroupType = GroupType.Personal });
            this.Org.GroupManager.Create(this.User1, new GroupCreateInfo { Name = personalGroup1.Name, GroupType = GroupType.Personal });
            //Get
            Assert.AreEqual(group1, this.Org.GroupManager.GetGroupById(group1.ID));

            this.Org.GroupManager.Delete(this.Admin, group1.ID);
        }

        [Test]
        public void GroupTest()
        {
            Group group1 = this.Org.GroupManager.Create(this.Admin, new GroupCreateInfo { Name = Guid.NewGuid().ToString(),  });
            

            //change
            GroupChangeInfo changeInfo = new GroupChangeInfo(group1.MapGroupInfo());
            changeInfo.Remark = "r1";
            group1.Change(this.Admin, changeInfo);
            Assert.AreEqual("r1", group1.Remark);

            //members
            Department testDept1 = this.Org.DepartmentManager.Create(this.Admin, new DepartmentCreateInfo { Name = Guid.NewGuid().ToString(), ManagerPositionInfo = new PositionCreateInfo { Name = Guid.NewGuid().ToString(), ParentId = this.Org.PositionManager.TopPosition.ID } });
            User testUser1 = this.Org.UserManager.Create(this.Admin, new UserCreateInfo { Account = Guid.NewGuid().ToString(), Password = "edoc2", Name = Guid.NewGuid().ToString(), MainPositionId = this.Org.PositionManager.TopPosition.ID });
            Position testPositoin1 = this.Org.PositionManager.Create(this.Admin, new PositionCreateInfo { Name = Guid.NewGuid().ToString(), ParentId = this.Org.PositionManager.TopPosition.ID });
            Group group2 = this.Org.GroupManager.Create(this.Admin, new GroupCreateInfo { Name = Guid.NewGuid().ToString(),  });

            group1.AddGroup(this.Admin, group2);
            Assert.AreEqual(1, group1.Groups.Count);

            group1.AddPosition(this.Admin, testPositoin1);
            Assert.AreEqual(1, group1.Positions.Count);

            group1.AddUser(this.Admin, testUser1);
            Assert.AreEqual(1, group1.Users.Count);

            group1.ClearMembers(this.Admin);
            Assert.AreEqual(0, group1.Groups.Count);
            Assert.AreEqual(0, group1.Positions.Count);
            Assert.AreEqual(0, group1.Users.Count);
        }

        [Test]
        public void AuthenticationTest()
        {
            string token;
            SignInResult result = Org.AuthenticationManager.TrySignIn("admin", "1", "127.0.0.1", out token);
            Assert.IsNull(token);
            Assert.AreEqual(SignInResult.WrongPassword, result);

            //Lock
            Admin.Lock(Admin);
            result = Org.AuthenticationManager.TrySignIn("admin", "edoc2", "127.0.0.1", out token);
            Assert.IsNull(token);
            Assert.AreEqual(SignInResult.AccountLocked, result);

            //Logoff
            Admin.Logoff(Admin);
            result = Org.AuthenticationManager.TrySignIn("admin", "edoc2", "127.0.0.1", out token);
            Assert.IsNull(token);
            Assert.AreEqual(SignInResult.AccountLogoffed, result);

            //Normal
            Admin.Activate(Admin);
            result = Org.AuthenticationManager.TrySignIn("admin", "edoc2", "127.0.0.1", out token);
            Assert.IsNotNull(token);
            Assert.AreEqual(SignInResult.OK, result);
            Assert.IsTrue(Org.AuthenticationManager.TryAuthenticateToken(token));

            //AccountNotFound
            result = Org.AuthenticationManager.TrySignIn("a", "edoc2", "127.0.0.1", out token);
            Assert.IsNull(token);
            Assert.AreEqual(SignInResult.AccountNotFound, result);
        }

        [Test]
        public void FunctionPermissionManagerTest()
        {
            FunctionCreateInfo createInfo = new FunctionCreateInfo();
            createInfo.ID = Guid.NewGuid().ToString();
            createInfo.Name = Guid.NewGuid().ToString();
            Function topFunction = this.Org.FunctionManager.Create(createInfo);
            Assert.IsNotNull(topFunction);

            FunctionCreateInfo createInfo1 = new FunctionCreateInfo();
            createInfo1.ID = Guid.NewGuid().ToString();
            createInfo1.Name = Guid.NewGuid().ToString();
            createInfo1.ParentId = topFunction.ID;
            Function function = this.Org.FunctionManager.Create(createInfo1);

            List<Function> functions = this.Org.FunctionManager.GetChildren(topFunction);
            Assert.AreEqual(1, functions.Count);
            Assert.AreEqual(function, functions[0]);

            Function function1 = this.Org.FunctionManager.GetFunctionInfoById(createInfo1.ID);
            Assert.AreEqual(function, function1);
        }

        [Test]
        public void FunctionPermissionTest()
        {
            FunctionCreateInfo createInfo = new FunctionCreateInfo();
            createInfo.ID = Guid.NewGuid().ToString();
            createInfo.Name = Guid.NewGuid().ToString();
            Function topFunction = this.Org.FunctionManager.Create(createInfo);
            Assert.IsNotNull(topFunction);

            FunctionCreateInfo createInfo1 = new FunctionCreateInfo();
            createInfo1.ID = Guid.NewGuid().ToString();
            createInfo1.Name = Guid.NewGuid().ToString();
            createInfo1.ParentId = topFunction.ID;
            Function function1 = this.Org.FunctionManager.Create(createInfo1);

            FunctionCreateInfo createInfo2 = new FunctionCreateInfo();
            createInfo2.ID = Guid.NewGuid().ToString();
            createInfo2.Name = Guid.NewGuid().ToString();
            createInfo2.ParentId = topFunction.ID;
            Function function2 = this.Org.FunctionManager.Create(createInfo2);

            User testUser1 = this.Org.UserManager.Create(this.Admin, new UserCreateInfo { Account = Guid.NewGuid().ToString(), Password = "edoc2", Name = Guid.NewGuid().ToString(), MainPositionId = this.Org.PositionManager.TopPosition.ID });
            User testUser2 = this.Org.UserManager.Create(this.Admin, new UserCreateInfo { Account = Guid.NewGuid().ToString(), Password = "edoc2", Name = Guid.NewGuid().ToString(), MainPositionId = this.Org.PositionManager.TopPosition.ID });
            Group group1 = this.Org.GroupManager.Create(this.Admin, new GroupCreateInfo { Name = Guid.NewGuid().ToString(),  });
            group1.AddUser(this.Admin, testUser2);

            function1.Add(testUser1, true);
            function1.Add(group1, true);

            Assert.IsTrue(function1.HasPermission(testUser1));
            Assert.IsTrue(function1.HasPermission(testUser2));
        }
    }
}
