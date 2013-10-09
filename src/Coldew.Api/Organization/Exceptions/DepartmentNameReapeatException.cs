using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.Serialization;

namespace Coldew.Api.Organization.Exceptions
{
    [Serializable]
    public class DepartmentNameReapeatException : OrganizationException
    {
        public DepartmentNameReapeatException()
        {
            this.ExceptionMessage = "部门名称重复!";
        }

        public DepartmentNameReapeatException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {

        }

        public override string Message
        {
            get
            {
                if (this.StringResouceProvider != null)
                {
                    return this.StringResouceProvider.GetString("ex_DepartmentNameReapeat");
                }
                return base.Message;
            }
        }
    }
}
