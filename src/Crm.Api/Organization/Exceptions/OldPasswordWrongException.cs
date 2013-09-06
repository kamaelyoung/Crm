using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.Serialization;

namespace Crm.Api.Organization.Exceptions
{
    [Serializable]
    public class OldPasswordWrongException : OrganizationException
    {
        public OldPasswordWrongException()
        {
            this.ExceptionMessage = "原来密码不正确!";
        }

        public OldPasswordWrongException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {

        }

        public override string Message
        {
            get
            {
                if (this.StringResouceProvider != null)
                {
                    return this.StringResouceProvider.GetString("ex_OldPasswordWrong");
                }
                return base.Message;
            }
        }
    }
}
