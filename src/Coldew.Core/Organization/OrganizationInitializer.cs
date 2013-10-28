using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Coldew.Data.Organization;
using Coldew.Api.Organization;

namespace Coldew.Core.Organization
{
    public class OrganizationInitializer
    {
        OrganizationManagement orgManager;

        public OrganizationInitializer(ColdewManager coldewManager)
        {
            this.orgManager = coldewManager.OrgManager;
            this.Init();
        }

        public OrganizationInitializer(OrganizationManagement orgManager)
        {
            this.orgManager = orgManager;
            this.Init();
        }

        void Init()
        {
            try
            {
                if (this.orgManager.DepartmentManager.TopDepartment == null)
                {
                    orgManager.Logger.Info("start init");
                    orgManager.DepartmentManager.Create(orgManager.System,
                            new DepartmentCreateInfo
                            {
                                Name = "销售总监",
                                ManagerPositionInfo = new PositionCreateInfo
                                {
                                    Name = "销售总监"
                                }
                            });

                    User admin = orgManager.UserManager.Create(orgManager.System, new UserCreateInfo
                    {
                        Name = "Administrator",
                        Account = "admin",
                        Password = "123456",
                        Role = UserRole.Administrator,
                        Status = UserStatus.Normal
                    });
                }
            }
            catch(Exception ex)
            {
                orgManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }
    }
}
