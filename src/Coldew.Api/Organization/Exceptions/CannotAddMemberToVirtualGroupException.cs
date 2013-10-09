using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Coldew.Api.Organization.Exceptions
{
    [Serializable]
    public class CannotAddMemberToVirtualGroupException : OrganizationException
    {
        string _groupName;

        public CannotAddMemberToVirtualGroupException(string groupName)
        {
            this.ExceptionMessage = "无法将成员加入到系统工作组中!";
            this._groupName = groupName;
        }

        public CannotAddMemberToVirtualGroupException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            this._groupName = info.GetString("_groupName");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("_groupName", this._groupName);
        }

        public override string Message
        {
            get
            {
                if (this.StringResouceProvider != null)
                {
                    return string.Format(this.StringResouceProvider.GetString("ex_CannotAddMemberToSystemGroup"), this._groupName);
                }
                return base.Message;
            }
        }
    }
}
