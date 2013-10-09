using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.Serialization;

namespace Coldew.Api.Organization.Exceptions
{
    [Serializable]
    public class DepartmentHasUserDeleteException: OrganizationException
    {
        public DepartmentHasUserDeleteException()
        {
            this.ExceptionMessage = "部门中包含有用户无法删除!";
        }

        public DepartmentHasUserDeleteException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {

        }

        public override string Message
        {
            get
            {
                if (this.StringResouceProvider != null)
                {
                    return this.StringResouceProvider.GetString("ex_DepartmentHasUserDelete");
                }
                return base.Message;
            }
        }

    }
}
