using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm.Api.Organization
{
    [Serializable]
    public class MemberFunctionInfo
    {
        public int ID { set; get; }

        public string MemberId { set; get; }

        public MemberType MemberType { set; get; }

        public string MemberName { set; get; }

        public bool HasPermission { set; get; }
    }
}
