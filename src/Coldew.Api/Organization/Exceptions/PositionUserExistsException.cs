using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.Serialization;

namespace Coldew.Api.Organization.Exceptions
{
    [Serializable]
    public class PositionUserExistsException : OrganizationException
    {
        string _userName;
        public PositionUserExistsException(string userName)
        {
            this.ExceptionMessage = string.Format("职位已经存在用户 {0}", userName);
            this._userName = userName;
        }

        public PositionUserExistsException(SerializationInfo info, StreamingContext context)
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
                    return string.Format(this.StringResouceProvider.GetString("ex_PositionUserExists"), this._userName);
                }
                return base.Message;
            }
        }
    }
}
