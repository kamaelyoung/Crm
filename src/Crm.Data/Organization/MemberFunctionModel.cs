using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crm.Data.Organization
{
    public class MemberFunctionModel
    {
        public MemberFunctionModel()
        {

        }

        public virtual int ID { get; set; }

        public virtual string FunctionId { get; set; }

        public virtual string MemberId { get; set; }

        public virtual int MemberType { get; set; }

        public virtual bool HasPermission{ get; set; }
    }
}
