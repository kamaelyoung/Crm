using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Api.Organization;

namespace Crm.Core.Organization
{
    public class OrganizationInitializationModel
    {

        public TopDepartmentInitModel TopDepartment { set;get; }

        public List<UserInitializationModel> Users { set; get; }

        public List<GroupInitializationModel> Groups { set; get; }

        public List<FunctionInitializationModel> Functions { set; get; }

        public List<ConfigInitializationModel> Configs { set; get; }
    }

    public class GroupInitializationModel
    {
        public string Code { set; get; }

        public string Name { set; get; }

        public GroupType Type { set; get; }

        public List<string> FunctionCategoryIds { set; get; }

        public List<string> FunctionIds { set; get; }
    }

    public class FunctionInitializationModel
    {
        public string ID { set; get; }

        public string Name { set; get; }

        public string Url { set; get; }

        public int Sort { set; get; }

        public string IconClass { set; get; }

        public List<FunctionInitializationModel> Children { set; get; }
    }

    public class ConfigInitializationModel
    {
        public string Name { set; get; }

        public string Value { set; get; }
    }

    public class UserInitializationModel
    {
        public string Name { set; get; }

        public string Account { set; get; }

        public string Password { set; get; }

        public UserGender Gender { set; get; }

        public UserRole Role { set; get; }
    }

    public class TopDepartmentInitModel
    {
        public string Name { set; get; }

        public string Code { set; get; }

        public string ManagerPositionName { set; get; }

        public string ManagerPositionCode { set; get; }
    }
}
