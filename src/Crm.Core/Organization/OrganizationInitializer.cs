using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Crm.Data.Organization;
using Crm.Api.Organization;

namespace Crm.Core.Organization
{
    public class OrganizationInitializer
    {
        OrganizationManagement orgManager;
        OrganizationChildInitializer[] childinitlizers;

        public OrganizationInitializer(CrmManager crmManager)
        {
            this.orgManager = crmManager.OrgManager;
            this.Init();
        }

        public OrganizationInitializer(OrganizationManagement orgManager, params OrganizationChildInitializer[] childinitlizers)
        {
            this.orgManager = orgManager;
            this.childinitlizers = childinitlizers;
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

                    if (this.childinitlizers != null)
                    {
                        foreach (OrganizationChildInitializer childIniter in childinitlizers)
                        {
                            childIniter.Init(orgManager);
                        }
                    }
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
