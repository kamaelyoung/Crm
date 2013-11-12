using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core.Organization;

namespace Coldew.Core
{
    public class MetadataOrgMember : MetadataMember
    {
        Member _member;
        public MetadataOrgMember(Member member)
        {
            this._member = member;
        }

        public override bool Contains(Metadata metadata, User user)
        {
            return this._member.Contains(user);
        }

        public override string Serialize()
        {
            return string.Format("org:{0}", this._member.ID);
        }
    }
}
