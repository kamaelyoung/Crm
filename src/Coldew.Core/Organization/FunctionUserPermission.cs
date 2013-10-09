using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Data.Organization;
using Coldew.Api.Organization;

namespace Coldew.Core.Organization
{
    public class FunctionUserPermission
    {
        OrganizationManagement _orgManager;
        public FunctionUserPermission(int id, User user, bool hasPermission, OrganizationManagement orgManager)
        {
            _orgManager = orgManager;
            this.ID = id;
            this.User = user;
            this.HasPermission = hasPermission;
        }

        public int ID { private set; get; }

        public User User { private set; get; }

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
            info.MemberId = this.User.ID;
            info.MemberType = MemberType.User; 
            info.MemberName = this.User.Name;
            info.HasPermission = this.HasPermission;
            return info;
        }
    }
}
