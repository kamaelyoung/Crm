using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crm.Data.Organization;
using Crm.Api.Organization;

namespace Crm.Core.Organization
{
    public class FunctionGroupPermission
    {
        OrganizationManagement _orgManager;
        public FunctionGroupPermission(int id, Group group, bool hasPermission, OrganizationManagement orgManager)
        {
            _orgManager = orgManager;
            this.ID = id;
            this.Group = group;
            this.HasPermission = hasPermission;
        }

        public int ID { private set; get; }

        public Group Group { private set; get; }

        public bool HasPermission { private set; get; }

        public void Modify(bool hasPermission)
        {
            MemberFunctionModel funModel = NHibernateHelper.CurrentSession.Get<MemberFunctionModel>(this.ID);
            funModel.HasPermission = hasPermission;
            NHibernateHelper.CurrentSession.Update(funModel);
            NHibernateHelper.CurrentSession.Flush();

            this.HasPermission = hasPermission;
        }

        public MemberFunctionInfo Map()
        {
            MemberFunctionInfo info = new MemberFunctionInfo();
            info.ID = this.ID;
            info.MemberId = this.Group.ID;
            info.MemberType = MemberType.Group;
            info.MemberName = this.Group.Name;
            info.HasPermission = this.HasPermission;
            return info;
        }
    }
}
