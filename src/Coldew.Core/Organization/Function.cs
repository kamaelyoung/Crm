using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Data.Organization;
using System.Collections.ObjectModel;
using NHibernate.Criterion;
using Coldew.Api.Organization;

namespace Coldew.Core.Organization
{
    public class Function
    {
        OrganizationManagement _orgManager;

        public Function(string id, string name, string url, string iconClass, int sort, List<Member> ownerMembers,  OrganizationManagement orgManager)
        {
            this._orgManager = orgManager;
            this.ID = id;
            this.Name = name;
            this.Url = url;
            this.IconClass = iconClass;
            this.Sort = sort;

            this._ownerMembers = ownerMembers;
            if (this._ownerMembers == null)
            {
                this._ownerMembers = new List<Member>();
            }
        }

        public string ID { private set; get; }

        public string Name { private set; get; }

        public string Url { private set; get; }

        public string IconClass { set; get; }

        public int Sort { private set; get; }

        private List<Member> _ownerMembers;

        public bool HasPermission(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (user.Role == UserRole.Administrator)
            {
                return true;
            }
            return this._ownerMembers.Any(x => x.Contains(user));
        }

        public FunctionInfo Map()
        {
            FunctionInfo info = new FunctionInfo();
            info.ID = this.ID;
            info.Name = this.Name;
            info.Url = this.Url;
            info.Sort = this.Sort;
            info.IconClass = this.IconClass;
            return info;
        }
    }
}
