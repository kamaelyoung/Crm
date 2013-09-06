using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.Serialization;

namespace Crm.Api.Organization.Exceptions
{
    [Serializable]
    public class GroupNameReapeatException : OrganizationException
    {
        public GroupNameReapeatException()
        {
            this.ExceptionMessage = "用户组名称重复!";
        }

        public GroupNameReapeatException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {

        }

        public override string Message
        {
            get
            {
                if (this.StringResouceProvider != null)
                {
                    return this.StringResouceProvider.GetString("ex_GroupNameReapeat");
                }
                return base.Message;
            }
        }
    }
}
