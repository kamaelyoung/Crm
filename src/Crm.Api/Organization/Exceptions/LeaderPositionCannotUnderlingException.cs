using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.Serialization;

namespace Crm.Api.Organization.Exceptions
{
    [Serializable]
    public class LeaderPositionCannotUnderlingException: OrganizationException
    {
        string _leaderPositionName; 
        string _positionName;
        public LeaderPositionCannotUnderlingException(string leaderPositionName, string positionName)
        {
            this.ExceptionMessage = string.Format("{0}是{1}的下级，不能再设置{1}为上级!",
                leaderPositionName, positionName);
            this._leaderPositionName = leaderPositionName;
            this._positionName = positionName;
        }

        public LeaderPositionCannotUnderlingException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            this._leaderPositionName = info.GetString("_leaderPositionName");
            this._positionName = info.GetString("_positionName");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("_leaderPositionName", this._leaderPositionName);
            info.AddValue("_positionName", this._positionName);
        }

        public override string Message
        {
            get
            {
                if (this.StringResouceProvider != null)
                {
                    return string.Format(this.StringResouceProvider.GetString("ex_LeaderPositionCannotUnderling"), this._leaderPositionName, this._positionName);
                }
                return base.Message;
            }
        }
    }
}
