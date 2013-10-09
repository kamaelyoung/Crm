using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.Serialization;

namespace Coldew.Api.Organization.Exceptions
{
    [Serializable]
    public class ContainsGroupException : OrganizationException
    {
        string _groupName;

        public ContainsGroupException(string groupName)
        {
            this.ExceptionMessage = string.Format("已经包含用户组{0}", groupName);
            this._groupName = groupName;
        }

        public ContainsGroupException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            this._groupName = info.GetString("groupName");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("groupName", this._groupName);
        }

        public override string Message
        {
            get
            {
                if (this.StringResouceProvider != null)
                {
                    return string.Format(this.StringResouceProvider.GetString("ex_containsGroup"), this._groupName);
                }
                return base.Message;
            }
        }
    }
}
