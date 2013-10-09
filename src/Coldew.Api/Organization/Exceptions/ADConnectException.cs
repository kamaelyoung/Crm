using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Coldew.Api.Organization.Exceptions
{
    [Serializable]
    public class ADConnectException : OrganizationException
    {
        public ADConnectException()
        {
            this.ExceptionMessage = "域服务器连接失败!";
        }

        public ADConnectException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            
        }

        public override string Message
        {
            get
            {
                if (this.StringResouceProvider != null)
                {
                    return this.StringResouceProvider.GetString("ex_adConnect");
                }
                return base.Message;
            }
        }
    }
}
