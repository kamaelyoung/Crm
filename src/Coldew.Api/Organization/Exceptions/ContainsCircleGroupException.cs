using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.Serialization;

namespace Coldew.Api.Organization.Exceptions
{
    [Serializable]
    public class ContainsCircleGroupException : OrganizationException
    {
        string _groupName;
        string _containsGroupName;
        public ContainsCircleGroupException(string groupName, string containsGroupName)
        {
            this.ExceptionMessage = string.Format("用户组{0}已经包含在用户组{1},不能循环添加!", containsGroupName, groupName);
            this._containsGroupName = containsGroupName;
            this._groupName = groupName;
        }

        public ContainsCircleGroupException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            this._groupName = info.GetString("_groupName");
            this._containsGroupName = info.GetString("_containsGroupName");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("_groupName", this._groupName);
            info.AddValue("_containsGroupName", this._containsGroupName);
        }

        public override string Message
        {
            get
            {
                if (this.StringResouceProvider != null)
                {
                    return string.Format(this.StringResouceProvider.GetString("ex_containsCircleGroup"), this._containsGroupName, this._groupName);
                }
                return base.Message;
            }
        }
    }
}
