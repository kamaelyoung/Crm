using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.Serialization;

namespace Crm.Api.Organization.Exceptions
{
    [Serializable]
    public class PasswordLengthInvalidException:OrganizationException
    {
        int _requirementLength;

        public PasswordLengthInvalidException(int requirementLength)
        {
            this.ExceptionMessage = string.Format("密码长度不符合要求，要求的长度{0}个字符!", 
                requirementLength);
            this._requirementLength = requirementLength;
        }

        public PasswordLengthInvalidException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            this._requirementLength = info.GetInt16("_requirementLength");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("_requirementLength", this._requirementLength);
        }

        public override string Message
        {
            get
            {
                if (this.StringResouceProvider != null)
                {
                    return string.Format(this.StringResouceProvider.GetString("ex_PasswordLengthInvalid"), this._requirementLength);
                }
                return base.Message;
            }
        }
    }
}
