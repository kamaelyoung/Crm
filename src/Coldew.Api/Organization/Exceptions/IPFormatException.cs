using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Coldew.Api.Organization.Exceptions
{
    [Serializable]
    public class IPFormatException : OrganizationException
    {
        string _ip;
        public IPFormatException(string ip)
        {
            this._ip = ip;
            this.ExceptionMessage = string.Format("IP地址{0}格式错误!", ip);
        }

        public IPFormatException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            this._ip = info.GetString("_ip");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("_ip", this._ip);
        }

        public override string Message
        {
            get
            {
                if (this.StringResouceProvider != null)
                {
                    return string.Format(this.StringResouceProvider.GetString("ex_ipFormat"), this._ip);
                }
                return base.Message;
            }
        }
    }
}
