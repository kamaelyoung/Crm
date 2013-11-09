using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Api.Organization;

namespace Coldew.Core.Organization
{
    public abstract class Member
    {
        public string ID { protected set; get; }

        public abstract MemberType Type { get; }

        public abstract List<Member> GetParents();

        public abstract List<Member> GetChildren();

        public abstract List<User> GetUsers(bool recursive);

        public abstract bool Contains(User user);
    }
}
