using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core.Organization;

namespace Coldew.Core
{
    public abstract class MetadataMember
    {
        public abstract bool Contains(Metadata metadata, User user);
        public abstract string Serialize();
        public static MetadataMember Create(string memberStr, OrganizationManagement orgManager)
        {
            MetadataMember metadataMember = null;
            string[] memberStrArray = memberStr.Split(':');
            if (memberStrArray[0] == "org")
            {
                Member member = orgManager.GetMember(memberStrArray[1]);
                if (member != null)
                {
                    metadataMember = new MetadataOrgMember(member);
                }
            }
            else
            {
                metadataMember = new MetadataFieldMember(memberStrArray[1]);
            }
            return metadataMember;
        }
    }
}
