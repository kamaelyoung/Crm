using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.Serialization;

namespace Coldew.Api.Organization.Exceptions
{
    [Serializable]
    public class ContainsUserException : OrganizationException
    {
        string _userName;

        public ContainsUserException(string userName)
        {
            this.ExceptionMessage = string.Format("已经包含了用户 {0}", userName);
            this._userName = userName;
        }

        public ContainsUserException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            this._userName = info.GetString("_userName");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("_userName", this._userName);
        }

        public override string Message
        {
            get
            {
                if (this.StringResouceProvider != null)
                {
                    return string.Format(this.StringResouceProvider.GetString("ex_ContainsUser"), this._userName);
                }
                return base.Message;
            }
        }
    }
}
