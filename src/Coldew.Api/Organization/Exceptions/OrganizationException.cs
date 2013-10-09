using System;
using System.Runtime.Serialization;
using Coldew.Api.Organization;
using Coldew.Api.Exceptions;

namespace Coldew.Api.Organization.Exceptions
{
    /// <summary>
    /// 组织异常
    /// </summary>
    [Serializable]
    public class OrganizationException : ColdewException
    {
        public OrganizationException()
        {
            this.ExceptionMessage = "组织操作异常!";
        }

        public OrganizationException(string message)
        {
            this.ExceptionMessage = message;
        }

        public OrganizationException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            this.ExceptionMessage = info.GetString("ExceptionMessage");
        }

        public IStringResouceProvider StringResouceProvider { set; get; }
    }
}
