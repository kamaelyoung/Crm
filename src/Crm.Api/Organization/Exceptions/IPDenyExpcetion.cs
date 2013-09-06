using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Crm.Api.Organization.Exceptions
{
    [Serializable]
    public class IPDenyExpcetion : OrganizationException
    {
        public IPDenyExpcetion()
        {
            this.ExceptionMessage = "受限IP地址!";
        }

        public IPDenyExpcetion(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            
        }
        public override string Message
        {
            get
            {
                if (this.StringResouceProvider != null)
                {
                    return this.StringResouceProvider.GetString("ex_ipDeny");
                }
                return base.Message;
            }
        }
    }
}
