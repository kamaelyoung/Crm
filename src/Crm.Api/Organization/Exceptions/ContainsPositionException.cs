using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.Serialization;

namespace Crm.Api.Organization.Exceptions
{
    [Serializable]
    public class ContainsPositionException : OrganizationException
    {
        string _positionName;
        public ContainsPositionException(string positionName)
        {
            this.ExceptionMessage = string.Format("已经包含职位{0}", positionName);
            this._positionName = positionName;
        }

        public ContainsPositionException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            this._positionName = info.GetString("_positionName");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("_positionName", this._positionName);
        }

        public override string Message
        {
            get
            {
                if (this.StringResouceProvider != null)
                {
                    return string.Format(this.StringResouceProvider.GetString("ex_containsPosition"), this._positionName);
                }
                return base.Message;
            }
        }
    }
}
