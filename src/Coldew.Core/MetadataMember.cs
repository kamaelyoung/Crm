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
        public static MetadataMember Create(string memberStr, ColdewObject cobject)
        {
            MetadataMember metadataMember = null;
            string[] memberStrArray = memberStr.Split(':');
            if (memberStrArray[0] == "org")
            {
                Member member = cobject.ColdewManager.OrgManager.GetMember(memberStrArray[1]);
                if (member != null)
                {
                    metadataMember = new MetadataOrgMember(member);
                }
            }
            else
            {
                Field field = cobject.GetFieldByCode(memberStrArray[1]);
                if (field != null)
                {
                    metadataMember = new MetadataFieldMember(field);
                }
            }
            return metadataMember;
        }
    }
}
