using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.Serialization;

namespace Crm.Api.Organization.Exceptions
{
    [Serializable]
    public class ContainsSelfGroupException : OrganizationException
    {
        public ContainsSelfGroupException()
        {
            this.ExceptionMessage = string.Format("不能将自己设置为工作组的成员!");
        }

        public ContainsSelfGroupException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {

        }

        public override string Message
        {
            get
            {
                if (this.StringResouceProvider != null)
                {
                    return this.StringResouceProvider.GetString("ex_containsSelfGroup");
                }
                return base.Message;
            }
        }
    }
}
