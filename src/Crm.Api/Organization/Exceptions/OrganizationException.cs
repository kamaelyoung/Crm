using System;
using System.Runtime.Serialization;
using Crm.Api.Organization;
using Crm.Api.Exceptions;

namespace Crm.Api.Organization.Exceptions
{
    /// <summary>
    /// 组织异常
    /// </summary>
    [Serializable]
    public class OrganizationException : CrmException
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
